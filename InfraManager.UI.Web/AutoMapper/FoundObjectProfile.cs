using AutoMapper;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.BLL.ServiceDesk.Search;
using InfraManager.BLL.ServiceDesk.WorkOrders;

namespace InfraManager.UI.Web.AutoMapper
{
    public class FoundObjectProfile : Profile // TODO: Этого класса тут быть не должно
    {
        public FoundObjectProfile()
        {
            CreateMap<FoundObject, InfraManager.Web.BLL.Search.FoundObject>();
            CreateMap<CallListItem, InfraManager.Web.BLL.Search.FoundObject>()
                .ForMember(f => f.Name, _ => _.MapFrom(c => c.Summary));
            CreateMap<CallFromMeListItem, InfraManager.Web.BLL.Search.FoundObject>()
                .ForMember(f => f.Name, _ => _.MapFrom(c => c.Summary));
            CreateMap<ProblemListItem, InfraManager.Web.BLL.Search.FoundObject>();
            CreateMap<WorkOrderListItem, InfraManager.Web.BLL.Search.FoundObject>();
            CreateMap<AllMassIncidentsReportItem, InfraManager.Web.BLL.Search.FoundObject>()
                .ForMember(f => f.Description, _ => _.MapFrom(c => c.FullDescription))
                .ForMember(f => f.ID, _ => _.MapFrom(c => c.IMObjID))
                .ForMember(f => f.Number, _ => _.MapFrom(c => c.ID));
        }
    }
}