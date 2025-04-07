using AutoMapper;
using InfraManager.DAL.Database.Import;
using InfraManager.ServiceBase.ImportService.DBService;

namespace InfraManager.BLL.Database.Import;

internal class UIDBConfigurationProfile : Profile
{
    public UIDBConfigurationProfile()
    {
        CreateMap<UIDBConfiguration, UIDBConfigurationOutputDetails>();

        CreateMap<UIDBConfigurationData, UIDBConfiguration>();

        CreateMap<UIDBConfigurationOutputDetails, UIDBConfigurationData>();
        
    }
}