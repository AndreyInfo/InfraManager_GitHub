using AutoMapper;
using InfraManager.BLL.ServiceDesk.CustomControl;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk
{
    public class CustomControlDataProfile : Profile
    {
        public CustomControlDataProfile()
        {
            CreateMap<MyCustomControlDataModel, CustomControlData>();
            CreateMap<UserCustomControlDataModel, CustomControlData>();           
        }
    }
}
