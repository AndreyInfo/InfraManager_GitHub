using InfraManager.BLL.Notification;
using InfraManager.BLL.Notification.Templates;
using InfraManager.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Settings;
using System.Web;
using InfraManager.ResourcesArea;
using InfraManager.BLL.Localization;

namespace InfraManager.BLL.Messages
{
    internal class NotificationTemplateBLL<TEntity, TTemplate> : INotificationTemplateBLL
        where TEntity : class
        where TTemplate : class, ITemplate<TEntity>

    {
        private readonly ISettingsBLL _settingsBLL;
        private readonly INotificationBLL _notificationBLL;
        private readonly IBuildEntityTemplate<TEntity, TTemplate> _templateBuilder;
        private readonly ILocalizeText _localizer;


        private static readonly Regex __parameterMatchRegex = new(@"(?<=\{\?)[\s\S][^{]*?(?=\})");

        private readonly Dictionary<ObjectClass, string> __patternPrifixes = new()
        {
            { ObjectClass.Call, "Call" },
            { ObjectClass.WorkOrder, "WorkOrder" },
            { ObjectClass.Problem, "Problem" },
            { ObjectClass.ChangeRequest, "RFC" },
            { ObjectClass.MassIncident, "MassIncident" }
        };
        
        private readonly Dictionary<string, Dictionary<string, PropertyInfo>> __propertyInfoCache = new();

        public NotificationTemplateBLL(
            ISettingsBLL settingsBLL,
            INotificationBLL notificationBLL,
            IBuildEntityTemplate<TEntity, TTemplate> templateBuilder,
            ILocalizeText localizer)
        {
            _settingsBLL = settingsBLL;
            _notificationBLL = notificationBLL;
            _templateBuilder = templateBuilder;
            _localizer = localizer;
        }

        public async Task<EMailTemplateDetails> GetEMailTemplateAsync(EMailTemplateRequest request, CancellationToken cancellationToken)
        {
            if (request.NotificationID == Guid.Empty)
                return await CreateDefaultTemplate(request.ClassID, request.ID, cancellationToken);

            var notification = await _notificationBLL.FindAsync(request.NotificationID, cancellationToken);

            var templateDto = await _templateBuilder.BuildAsync(request.ID, cancellationToken, request.UserID);

            var subject = await FillTemplateAsync(notification.Subject, request.ClassID, templateDto, cancellationToken);
            var body = await FillTemplateAsync(notification.Body, request.ClassID, templateDto, cancellationToken);

            return new EMailTemplateDetails { Subject = subject, Body = body, };
        }

        private async Task<EMailTemplateDetails> CreateDefaultTemplate(ObjectClass classID, Guid id, CancellationToken cancellationToken)
        {
            string subject = string.Empty;
            var templateDto = await _templateBuilder.BuildAsync(id, cancellationToken);
            if (templateDto != null)
            {
                var template = await (classID switch {
                    ObjectClass.Problem => _localizer.LocalizeAsync(nameof(Resources.Problem_EMailSubject), cancellationToken),
                    ObjectClass.Call => _localizer.LocalizeAsync(nameof(Resources.Call_EmailSubject), cancellationToken),
                    ObjectClass.WorkOrder => _localizer.LocalizeAsync(nameof(Resources.WorkOrder_EmailSubject), cancellationToken),
                    ObjectClass.ChangeRequest => _localizer.LocalizeAsync(nameof(Resources.RFC_EmailSubject), cancellationToken),
                    ObjectClass.MassIncident => _localizer.LocalizeAsync(nameof(Resources.MassIncident_EmailSubject), cancellationToken),
                    _ => Task.FromResult(string.Empty)
                } );
                subject = await FillTemplateAsync(template, classID, templateDto, cancellationToken);
            }
            return new EMailTemplateDetails { Body = string.Empty, Subject = subject };
        }

        // todo: Этот метод копипаста из legacy. Нужно переписать.
        private async Task<string> FillTemplateAsync(string template, ObjectClass classID, TTemplate @object, CancellationToken cancellationToken)
        {
            const string __openBracket = "{?";
            const string __closeBracket = "}";
            const string __separator = ".";

            Dictionary<string, PropertyInfo> propertyInfos = GetPropertyInfos(@object.GetType());
            var prefix = __patternPrifixes.ContainsKey(classID) ? __patternPrifixes[classID] : classID.ToString();
            var patternPrefix = prefix == null ? null : $"{prefix}{__separator}";
            while (true)
            {
                var parameterMatch = __parameterMatchRegex.Match(template);
                if (!parameterMatch.Success)
                    break;
                string svalue;
                string pattern;
                if (patternPrefix != null && parameterMatch.Value.Contains(patternPrefix))
                {
                    var parameter = parameterMatch.Value.Substring(patternPrefix.Length, parameterMatch.Value.Length - patternPrefix.Length);
                    svalue = string.Format("[Значение параметра '{0}' неопределено.]", parameter);
                    if (parameter.StartsWith("SetGrade["))
                    {
                        string page = (await _settingsBLL.ConvertValueAsync(SystemSettings.CallEmailTemplateID, cancellationToken)) as string;
                        string baseObjectIDString = @object.GetType().GetProperty("ID").GetValue(@object).ToString();
                        string email;
                        var emailMatch = Regex.Match(parameter, @"(?<=email\=)([\s\S][^\;]+);", RegexOptions.Compiled);
                        var orientationMatch = Regex.Match(parameter, @"(?<=orientation\=)([\s\S][^\;]+);", RegexOptions.Compiled);
                        bool verticalOrientation = true;
                        if (orientationMatch.Success)
                        {
                            var orientation = orientationMatch.Value.TrimEnd(';');
                            if (orientation.Equals("horizontal", StringComparison.OrdinalIgnoreCase))
                                verticalOrientation = false;
                        }
                        if (emailMatch.Success)
                        {
                            email = emailMatch.Value.TrimEnd(';');
                            var sb = new StringBuilder();
                            if (verticalOrientation)
                                sb.Append("<style>div.vertical > grade > * {vertical-align:middle;} div.vertical > grade {display: block; margin-top: 5px; margin-left: 10px;}</style><div class=\"vertical\">\r\n");
                            else
                                sb.Append("<style>div.horizontal > grade > * {vertical-align:middle; margin-top:5px;} div.horizontal > grade {display: inline-block; margin-right: 10px;}</style><div class=\"horizontal\">\r\n");
                            var matches = Regex.Matches(parameter, @"(\d+)-([\s\S][^\;]*);", RegexOptions.Compiled);
                            foreach (Match match in matches)
                                if (match.Success)
                                {
                                    sb.Append("<grade>\r\n");
                                    sb.Append(string.Format("<a href=\"{0}?token={1}&grade={2}&email={3}\">",
                                                                page,
                                                                System.Web.HttpUtility.UrlEncode(baseObjectIDString),
                                                                match.Groups[1].Value,
                                                                System.Web.HttpUtility.UrlEncode(email)));
                                    sb.Append(match.Groups[2].Value);
                                    sb.Append("</a>\r\n");
                                    sb.Append("</grade>\r\n");
                                    if (verticalOrientation)
                                        sb.Append("</br>\r\n");
                                }
                            sb.Append("</div>\r\n");
                            svalue = sb.ToString();
                        }
                    }
                    else if (propertyInfos.TryGetValue(parameter, out var propertyInfo) && propertyInfo != null)
                    {
                        var ovalue = propertyInfo.GetValue(@object, null);
                        svalue = ovalue == null ? string.Empty : ovalue.ToString();
                        if (!propertyInfo.Name.ToUpper().StartsWith("HTML"))
                        {
                            svalue = svalue != null ? svalue.ToWebMultiline() : svalue;
                        }
                    }
                    else
                        svalue = string.Format("[Значение параметра '{0}' неопределено.]", parameter);
                    //
                    pattern = string.Concat(__openBracket, prefix, __separator, parameter, __closeBracket);
                }
                else
                {
                    svalue = string.Format("[Параметр '{0}' ошибочен.]", parameterMatch.Value);
                    pattern = string.Concat(__openBracket, parameterMatch.Value, __closeBracket);
                }
                template = template.Replace(pattern, svalue, StringComparison.OrdinalIgnoreCase);
            }

            return template;
        }

        private Dictionary<string, PropertyInfo> GetPropertyInfos(Type type)
        {
            if (__propertyInfoCache.ContainsKey(type.FullName))
                return __propertyInfoCache[type.FullName];
            
            var result = new Dictionary<string, PropertyInfo>();
            foreach (var propertyInfo in type.GetProperties())
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof(TemplateParameterAttribute), true);
                if (attributes.Length > 0)
                    result[propertyInfo.Name] = propertyInfo;
            }

            __propertyInfoCache[type.FullName] = result;

            return result;
        }
    }
}
