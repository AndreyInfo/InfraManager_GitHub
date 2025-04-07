using InfraManager.DependencyInjection;
using System;
using System.Linq;

namespace InfraManager.BLL.Settings;

public static class ServiceMappingExtensions
{
    public static ServiceMapping<SystemSettings, IConvertSettingValue>
    MapSystemSettingConverters(this ServiceMapping<SystemSettings, IConvertSettingValue> mapping)
    {
        var settings = Enum.GetValues(typeof(SystemSettings)).Cast<SystemSettings>();
        foreach (var setting in settings)
        {
            if (setting.HasSystemSettingTypeMappingAttribute())
            {
                mapping.Map(setting.GetConverterType()).To(setting);
            }
        }
        return mapping;
    }
}
