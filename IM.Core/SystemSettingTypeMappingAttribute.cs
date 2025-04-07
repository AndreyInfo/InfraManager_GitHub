using System;

namespace InfraManager;

public class SystemSettingTypeMappingAttribute : Attribute
{
    public readonly Type ValueType;
    public SystemSettingTypeMappingAttribute(Type type)
    {
        ValueType = type;
    }
}
