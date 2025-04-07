using System;

namespace InfraManager.BLL.Settings;

public static class SystemSettingsExtenions
{
    /// <summary>
    /// Initiate IConvertSettingValue<ConcreteType>, where ConcreteType is ValueType of SystemSettingTypeMappingAttribute
    /// throws if SystemSettings setting hasn't SystemSettingTypeMappingAttribute
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="provider"></param>
    /// <returns> typeof(IConvertSettingValue<ConcreteType>)</returns>
    public static Type GetConverterType(this SystemSettings setting)
    {
        var type = GetFirstOrDefaultAttribute<SystemSettingTypeMappingAttribute>(setting).ValueType;
        return typeof(IConvertSettingValue<>).MakeGenericType(type);
    }

    public static bool HasSystemSettingTypeMappingAttribute(this SystemSettings setting)
        => GetFirstOrDefaultAttribute<SystemSettingTypeMappingAttribute>(setting) != default;

    private static T GetFirstOrDefaultAttribute<T>(Enum enumVal) where T : Attribute
    {
        var memInfo = enumVal.GetType().GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return attributes.Length > 0 ? (T)attributes[0] : default;
    }
}
