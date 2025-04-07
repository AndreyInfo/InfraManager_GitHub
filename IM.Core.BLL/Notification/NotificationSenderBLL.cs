using InfraManager.BLL.Messages;
using InfraManager.BLL.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;

namespace InfraManager.BLL.Notification
{
    public class NotificationSenderBLL : INotificationSenderBLL, ISelfRegisteredService<INotificationSenderBLL>
    {
        private readonly ISendEMailBLL _sendEMailBLL;
        private readonly IServiceMapper<ObjectClass, INotificationTemplateBLL> _templateMapper;
        private readonly IEMailProtocolBLL _eMailProtocolBLL;
        private readonly INotificationBLL _notificationBLL;
        private readonly IReadonlyRepository<User> _users;
        private readonly ILogger _logger;
        private readonly ISettingsBLL _settingsBLL;

        public NotificationSenderBLL(
            ISendEMailBLL sendEMailBLL,
            IServiceMapper<ObjectClass, INotificationTemplateBLL> templateMapper,
            IEMailProtocolBLL eMailProtocolBLL,
            INotificationBLL notificationBLL,
            IReadonlyRepository<User> users,
            ILogger<NotificationSenderBLL> logger,
            ISettingsBLL settingsBLL)
        {
            _sendEMailBLL = sendEMailBLL;
            _templateMapper = templateMapper;
            _eMailProtocolBLL = eMailProtocolBLL;
            _notificationBLL = notificationBLL;
            _users = users;
            _logger = logger;
            _settingsBLL = settingsBLL;
        }

        public async Task<bool> SendNotificationAsync(SystemSettings setting, InframanagerObject @object, CancellationToken cancellationToken)
        {
            if (Guid.TryParse((await _settingsBLL.ConvertValueAsync(setting, cancellationToken))?.ToString(), out var notificationID))
            {
                return await SendNotificationAsync(notificationID, @object, cancellationToken);
            }

            return false;
        }

        public async Task<bool> SendSeparateNotificationsAsync(SystemSettings setting, InframanagerObject @object,
            CancellationToken cancellationToken)
        {
            if (Guid.TryParse((await _settingsBLL.ConvertValueAsync(setting, cancellationToken))?.ToString(),
                    out var notificationID))
            {
                return await SendSeparateNotificationsAsync(notificationID, @object, cancellationToken);
            }

            return false;
        }

        private async Task<bool> SendNotificationAsync(Guid notificationID, InframanagerObject @object, CancellationToken cancellationToken)
        {
            var templateBLL = _templateMapper.Map(@object.ClassId);
            var template = await templateBLL.GetEMailTemplateAsync(
                new EMailTemplateRequest
                {
                    NotificationID = notificationID,
                    ClassID = @object.ClassId,
                    ID = @object.Id,
                }, cancellationToken);
            var notification = await _notificationBLL.FindAsync(notificationID, cancellationToken);

            if (notification == null)
                return false;

            var emails = await GetEmailAddresses(@object, notification, cancellationToken);
            if (!emails.Any()) return false;
            var mail = new SendEMailData()
            {
                Subject = template.Subject,
                HtmlBody = template.Body,
                ToAddresses = String.Join(',', emails),
            };
            return await _sendEMailBLL.SendEMailAsync(mail, cancellationToken);

        }

        private async Task<bool> SendSeparateNotificationsAsync(Guid notificationID, InframanagerObject @object,
            CancellationToken cancellationToken)
        {
            var notification = await _notificationBLL.FindAsync(notificationID, cancellationToken);
            if (notification == null)
                return false;
            var templateBLL = _templateMapper.Map(@object.ClassId);
            bool result = false;
            var emails = await GetEmailAddresses(@object, notification, cancellationToken);
            foreach (var email in emails)
            {
                var user = await _users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
                var template = await templateBLL.GetEMailTemplateAsync(
                    new EMailTemplateRequest
                    {
                        NotificationID = notificationID,
                        ClassID = @object.ClassId,
                        ID = @object.Id,
                        UserID = user.IMObjID
                    }, cancellationToken);
                var mail = new SendEMailData()
                {
                    Subject = template.Subject,
                    HtmlBody = template.Body,
                    ToAddresses = email
                };
                result |= await _sendEMailBLL.SendEMailAsync(mail, cancellationToken);
            }
            return result;
        }

        private async Task<List<string>> GetEmailAddresses(InframanagerObject @object, NotificationDetails notification,
            CancellationToken cancellationToken)
        {
            var emails = new List<string>();
            foreach (var recipient in notification.NotificationRecipient)
            {
                switch (recipient.Type)
                {
                    case RecipientType.Email:
                        _logger.LogTrace(
                            $"EmailProtocol.ConstructSMTPMessage(){Environment.NewLine}Adding '{recipient.Name}' as a recipient.");
                        emails.Add(recipient.Name);
                        break;
                    case RecipientType.BusinessRole:
                        var emailList = await _eMailProtocolBLL.GetEMAilsAsync(
                            new WebApi.Contracts.Models.EMailProtocol.EMailListRequest
                            {
                                NotificationID = notification.ID,
                                ObjectID = @object.Id,
                                BusinessRole = recipient.BusinessRoleID,
                            }, cancellationToken);
                        var userDr =
                            emailList.AsEnumerable()
                                .FirstOrDefault(x => x.ClassID == ObjectClass.User); //Ищем исполнителя
                        //
                        if (userDr != null) //попадаем сюда если RecipientScope = Object
                        {
                            var userDrWithRole =
                                emailList.FirstOrDefault(x =>
                                    x.ClassID == ObjectClass.User &&
                                    x.HasUserRole == true); //Может ли исполнитель получать оповещение?
                            //
                            if (userDrWithRole != null)
                            {
                                foreach (var item in emailList)
                                {
                                    if (!string.IsNullOrWhiteSpace(item.EMail) && !emails.Contains(item.EMail) &&
                                        item.ClassID == ObjectClass.User)
                                    {
                                        _logger.LogTrace(
                                            $"EmailProtocol.ConstructSMTPMessage(){Environment.NewLine}Adding '{item.EMail}' as a recipient.");
                                        //
                                        emails.Add(item.EMail);
                                    }
                                }
                            }
                        }
                        else //попадаем сюда если RecipientScope = Object или All
                        {
                            foreach (var item in emailList)
                            {
                                string email = item.EMail;
                                if (!emails.Contains(email))
                                {
                                    //
                                    _logger.LogTrace(
                                        $"EmailProtocol.ConstructSMTPMessage(){Environment.NewLine}Adding '{email}' as a recipient.");
                                    emails.Add(email);
                                }
                            }
                        }

                        break;
                }
            }

            return emails;
        }
    }
}