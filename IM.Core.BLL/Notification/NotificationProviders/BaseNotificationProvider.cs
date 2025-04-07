using System;
using System.Collections.Generic;
using System.Reflection;
using InfraManager.Core;

namespace InfraManager.BLL.Notification.NotificationProviders;

internal class BaseNotificationProvider
{
    private const string __openBracket = "{?";
    private const string __closeBracket = "}";
    private const string __separator = ".";
    protected Dictionary<int, string> _businessRoles = new ();
    protected string _prefix;
    protected Type _type;
    
    //TODO подумать над изменением процесса получения имени
    protected string GetName(Enum value)
    {
        Type type = value.GetType();
        string name = Enum.GetName(type, value);
        if (name != null)
        {
            var field = type.GetField(name);
            if (field != null)
            {
                var attr =
                    Attribute.GetCustomAttribute(field,
                        typeof(FriendlyNameAttribute)) as FriendlyNameAttribute;
                if (attr != null)
                {
                    return attr.Name;
                }
            }
        }
        return null;
    }

    private Dictionary<string, PropertyInfo> GetPropertiesInfo(Type type)
    {
        var propertiesInfo = new Dictionary<string, PropertyInfo>();
        foreach (var propertyInfo in type.GetProperties())
        {
            var attributes = propertyInfo.GetCustomAttributes(typeof(TemplateParameterAttribute), true);
            if (attributes.Length > 0)
                propertiesInfo[propertyInfo.Name] = propertyInfo;
        }
        return propertiesInfo;
    }

    protected ParameterTemplate[] BuildParameterTemplates()
    {
        if (string.IsNullOrEmpty(_prefix))
        {
            throw new NotSupportedException($"Prefix should be configured");
        }
        
        if (_type == null)
        {
            throw new NotSupportedException($"Type should be configured");
        }
        
        List<ParameterTemplate> parameterTemplates = new List<ParameterTemplate>();
        
        foreach (var propertyInfo in GetPropertiesInfo(_type))
        {
            parameterTemplates.Add(new ParameterTemplate
            {
                Name = ((TemplateParameterAttribute)propertyInfo.Value.GetCustomAttribute(
                    typeof(TemplateParameterAttribute)))!.DisplayName,
                Template = string.Concat(__openBracket, _prefix, __separator, propertyInfo.Key, __closeBracket)
            });
        }

        return parameterTemplates.ToArray();
    }
}