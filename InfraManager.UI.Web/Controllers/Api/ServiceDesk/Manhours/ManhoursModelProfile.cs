using AutoMapper;
using InfraManager.BLL.ServiceDesk.Manhours;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Manhours;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Manhours
{
    public class ManhoursModelProfile : Profile
    {
        public ManhoursModelProfile()
        {
            CreateMap<ManhoursWorkDetails, ManhoursWorkDetailsModel>()
                .ForMember(d => d.ManhoursList, mapper => mapper.MapFrom(d => d.Entries));
            CreateMap<ManhoursDetails, ManhoursDetailsModel>();
            CreateMap<ManhoursWorkDataModel, ManhoursWorkData>();
        }
    }
}
