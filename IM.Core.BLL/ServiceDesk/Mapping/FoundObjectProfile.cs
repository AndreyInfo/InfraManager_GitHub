using System;
using AutoMapper;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.BLL.ServiceDesk.Search;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.DAL.KnowledgeBase;

namespace InfraManager.BLL.ServiceDesk.Mapping
{
    public class FoundObjectProfile : Profile
    {
        public FoundObjectProfile()
        {
            CreateMap<WorkOrderDetails, FoundObject>();

            CreateMap<CallDetails, FoundObject>()
                .ForMember(x => x.ExecutorID,
                    mapper => mapper.MapFrom((_, ctx) =>
                        Guid.TryParse(_.ExecutorID, out var executorID) ? (Guid?)executorID : null))
                .ForMember(x => x.OwnerID,
                    mapper => mapper.MapFrom((_, ctx) =>
                        Guid.TryParse(_.OwnerID, out var ownerID) ? (Guid?)ownerID : null))
                .ForMember(x => x.ClientID,
                    mapper => mapper.MapFrom((_, ctx) =>
                        Guid.TryParse(_.ClientID, out var clientID) ? (Guid?)clientID : null))
                .ForMember(x => x.Name, mapper => mapper.MapFrom(_ => _.CallSummaryName));

            CreateMap<ProblemDetails, FoundObject>()
                .ForMember(x => x.OwnerID,
                    mapper => mapper.MapFrom((_, ctx) =>
                        Guid.TryParse(_.OwnerID, out var ownerID) ? (Guid?)ownerID : null))
                .ForMember(x => x.Name, mapper => mapper.MapFrom(_ => _.Summary));


            CreateMap<KBArticleShortItem, FoundObject>()
                .ForMember(x => x.OwnerID,
                    mapper =>
                        mapper.MapFrom((_, ctx) => _.AuthorId))
                .ForMember(x => x.ClassID, _ => _.MapFrom(item => ObjectClass.KBArticle));

            CreateMap<MassIncidentDetails, FoundObject>()
                .ForMember(f => f.OwnerID, _ => _.MapFrom(c => c.OwnedByUserID))
                .ForMember(f => f.Description, _ => _.MapFrom(c => c.Description))
                .ForMember(f => f.ID, _ => _.MapFrom(c => c.IMObjID))
                .ForMember(f => f.Number, _ => _.MapFrom(c => c.ID));
        }
    }
}