using AutoMapper;
using InfraManager.DAL.Database.Import;
using InfraManager.ServiceBase.ImportService.DBService;

namespace InfraManager.BLL.Database.Import;

internal class UIDBConnectionStringProfile : Profile
{
    public UIDBConnectionStringProfile()
    {
        CreateMap<UIDBConnectionString, UIDBConnectionStringOutputDetails>();

        CreateMap<UIDBConnectionStringData, UIDBConnectionString>();

        CreateMap<UIDBConnectionStringData, UIDBConnectionStringOutputDetails>().ReverseMap();
    }
}