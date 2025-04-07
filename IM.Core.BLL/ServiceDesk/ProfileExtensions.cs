using AutoMapper;
using InfraManager.BLL.ServiceDesk.ChangeRequests;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.BLL.ServiceDesk
{
    internal static class ProfileExtensions
    {
        public static void CreateCallReferenceMap<T>(this Profile profile) where T : CallReferenceListItemBase
        {
            profile.CreateMap<Call, T>()
                .ForMember(listItem => listItem.Client, mapper => mapper.MapFrom(Call.ClientFullName))
                .ForMember(listItem => listItem.TypeName, mapper => mapper.MapFrom(entity => entity.CallType.Name))
                .ForMember(listItem => listItem.ShortDescription, mapper => mapper.MapFrom(entity => entity.CallSummaryName));
        }

        public static void CreateChangeRequestReferenceMap<T>(this Profile profile) where T : ChangeRequestReferenceListItemBase
        {
            profile.CreateMap<ChangeRequest, T>()
                .ForMember(listItem => listItem.Owner, mapper => mapper.MapFrom(entity => entity.Owner.FullName))
                .ForMember(listItem => listItem.TypeName, mapper => mapper.MapFrom(entity => entity.Type.Name))
                .ForMember(listItem => listItem.Priority, mapper => mapper.MapFrom(entity => entity.Priority.Name))
                .ForMember(listItem => listItem.ShortDescription, mapper => mapper.MapFrom(entity => entity.Summary));
        }

        public static void CreateProblemReferenceMap<T>(this Profile profile) where T : ProblemReferenceListItemBase
        {
            profile.CreateMap<Problem, T>()
                .ForMember(listItem => listItem.Client, mapper => mapper.MapFrom(Problem.InitiatorFullName))
                .ForMember(listItem => listItem.TypeName, mapper => mapper.MapFrom(entity => entity.Type.Name))
                .ForMember(listItem => listItem.ShortDescription, mapper => mapper.MapFrom(entity => entity.Summary));
        }

        public static void CreateWorkOrderReferenceMap<T>(this Profile profile) where T : WorkOrderReferenceListItemBase
        {
            profile.CreateMap<WorkOrder, T>()
                .ForMember(listItem => listItem.ShortDescription, mapper => mapper.MapFrom(entity => entity.Name))
                .ForMember(listItem => listItem.TypeName, mapper => mapper.MapFrom(entity => entity.Type.Name));
        }
    }
}
