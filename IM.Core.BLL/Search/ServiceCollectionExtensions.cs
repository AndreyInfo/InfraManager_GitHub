using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.BLL.Search
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSearchers(this IServiceCollection services)
        {
            return services.AddMappingScoped(
                new ServiceMapping<string, IObjectSearcher>()
                    .Map<ServiceItemAndAttendanceSearcher>().To(ObjectSearchers.ServiceItemAndAttendanceSearcher)
                    .Map<GroupSearcher>().To(ObjectSearchers.QueueSearcher)
                    .Map<WebUserSearcher>().To(ObjectSearchers.WebUserSearcher)
                    .Map<WebUserSearcherNoTOZ>().To(ObjectSearchers.WebUserSearcherNoTOZ)
                    .Map<OwnerUserSearcher>().To(ObjectSearchers.OwnerUserSearcher)
                    .Map<ExecutorUserSearcher>().To(ObjectSearchers.ExecutorUserSearcher)
                    .Map<AccomplisherUserSearcher>().To(ObjectSearchers.AccomplisherUserSearcher)
                    .Map<UserEmailSearcher>().To(ObjectSearchers.UserWithEmailSearcher)
                    .Map<CallTypeSearcher>().To(ObjectSearchers.CallTypeSearcher)
                    .Map<ProblemTypeSearcher>().To(ObjectSearchers.ProblemTypeSearcher)
                    .Map<OrganizationSearcher>().To(ObjectSearchers.OrganizationSearcher)
                    .Map<ProblemCauseSearcher>().To(ObjectSearchers.ProblemCauseSearcher)
                    .Map<SubdivisionSearcher>().To(ObjectSearchers.SubDivisionSearcher)
                    .Map<SubdivisionNoTozSearcher>().To(ObjectSearchers.SubDivisionSearcherNoTOZ)
                    .Map<WorkOrderTypeSearcher>().To(ObjectSearchers.WorkOrderTypeSearcher)
                    .Map<NegotiationUserSearcher>().To(ObjectSearchers.NegotiationUserSearcher)
                    .Map<ChangeRequestCategorySearcher>().To(ObjectSearchers.RfcCategorySearcher)
                    .Map<ChangeRequestTypeSearcher>().To(ObjectSearchers.RfcTypeSearcher)
                    .Map<ChangeRequestServiceSearcher>().To(ObjectSearchers.RfcServiceSearcher)
                    .Map<TagSearcher>().To(ObjectSearchers.TagSearcher)
                    .Map<CallSummarySearcher>().To(ObjectSearchers.CallSummarySearcher)
                    .Map<UserForDeputySearcher>().To(ObjectSearchers.UserForDeputySearcher)
                    .Map<UserActivityTypeSearcher>().To(ObjectSearchers.UserActivityTypeSearcher)                    
                    .Map<GroupNoTozSearcher>().To(ObjectSearchers.QueueSearcherWithoutCurrentUser)
                    .Map<OrganizationNoTozWithoutCurrentUserSearcher>().To(ObjectSearchers.OrganizationSearcherNoTozWithoutCurrentUser)
                    .Map<SubdivisionNoTozWithoutCurrentUserSearcher>().To(ObjectSearchers.SubDivisionSearcherNoTozWithoutCurrentUser)
                    .Map<WorkplaceSearcher>().To(ObjectSearchers.WorkplaceSearcher)
                    .Map<RoomSearcher>().To(ObjectSearchers.RoomSearcher)
                    .Map<RackSearcher>().To(ObjectSearchers.RackSearcher)
                    .Map<UtilizerSearcher>().To(ObjectSearchers.UtilizerSearcher)
                    .Map<SolutionSearcher>().To(ObjectSearchers.SolutionSearcher)
                    .Map<RFCResultSearcher>().To(ObjectSearchers.RFCResultSearcher)
                    .Map<IncidentResultSearcher>().To(ObjectSearchers.IncidentResultSearcher)
                    .Map<UserSearcherForFormParameter>().To(ObjectSearchers.UserSearcherForFormParameter)
                    .Map<ServiceSearcher>().To(ObjectSearchers.ServiceSearcher)
                    .Map<MassIncidentOwnerSearcher>().To(ObjectSearchers.MassIncidentOwnerSearcher));
        }
    }
}
