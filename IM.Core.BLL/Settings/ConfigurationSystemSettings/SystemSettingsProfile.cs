using AutoMapper;
using InfraManager.BLL.AppSettings;

namespace InfraManager.BLL.Settings.ConfigurationSystemSettings;

public class SystemSettingsProfile : Profile
{
    public SystemSettingsProfile()
    {
        CreateMap<SystemWebSettings, SystemWebSettings>();
        
        CreateMap<string, ServiceSettings>()
            .ConstructUsing(x => new ServiceSettings(x));

        CreateMap<ServiceSettings, string>()
            .ConvertUsing(x => x.ToString());

        CreateMap<SystemSettingsJson, SystemSettingData>();
        
        CreateMap<SystemSettingData, SystemSettingsJson>();
    }
}