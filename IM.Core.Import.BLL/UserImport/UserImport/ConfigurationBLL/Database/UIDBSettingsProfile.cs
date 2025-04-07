using AutoMapper;
using InfraManager.DAL.Database.Import;
using InfraManager.ServiceBase.ImportService.DBService;

namespace InfraManager.BLL.Database.Import;

internal class UIDBSettingsProfile : Profile
{
    public UIDBSettingsProfile()
    {
        CreateMap<UIDBSettings, UIDBSettingsOutputDetails>();

        CreateMap<UIDBSettingsData, UIDBSettings>();

        CreateMap<UIDBSettingsOutputDetails, UIDBSettingsData>();
    }
}