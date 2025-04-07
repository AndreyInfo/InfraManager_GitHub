using AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk
{
    public class IncidentResultProfile : Profile
    {
        public IncidentResultProfile()
        {
            CreateMap<IncidentResult, IncidentResultListItemModel>();
            CreateMap<IncidentResultModel, IncidentResult>()
                .ConstructUsing(x => new IncidentResult(x.Name));
            CreateMap<IncidentResult, IncidentResultDetailsModel>();
        }
    }
}
