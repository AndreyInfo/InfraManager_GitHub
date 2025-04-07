using InfraManager.BLL.ELP;
 using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements.Events;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.BLL.ServiceDesk.Calls.Events;
using InfraManager.BLL.ServiceDesk.ChangeRequests;
using InfraManager.BLL.ServiceDesk.ChangeRequests.Events;
using InfraManager.BLL.ServiceDesk.CustomControl;
using InfraManager.BLL.ServiceDesk.KnowledgeBase;
using InfraManager.BLL.ServiceDesk.Manhours.Events;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.BLL.ServiceDesk.Negotiations;
using InfraManager.BLL.ServiceDesk.Negotiations.Events;
using InfraManager.BLL.ServiceDesk.Negotiations.StatusCalculation;
using InfraManager.BLL.ServiceDesk.Notes;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.BLL.ServiceDesk.Problems.Events;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.BLL.ServiceDesk.WorkOrders.Events;
using InfraManager.BLL.Workflow;
using InfraManager.BLL.WorkFlow;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.Message;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Calls;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.Negotiations;
using InfraManager.DAL.ServiceDesk.Problems;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using InfraManager.BLL.ServiceDesk.MassIncidents.Events;
using InfraManager.BLL.ServiceDesk.WorkOrders.WorkOrderReferenced;
using InfraManager.BLL.Settings.TableFilters;
using System;
using InfraManager.BLL.Asset;
using InfraManager.BLL.Users;
using InfraManager.BLL.Users.Events;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;

namespace InfraManager.BLL.ServiceDesk
{
    public static class ServiceCollectionExtensions
    {
        #region Entry point

        internal static IServiceCollection AddServiceDesk(this IServiceCollection services)
        {
            services
                .AddScoped<IValidateObjectState<Call>, WorkflowEntityStateValidator<Call>>()
                .AddScoped<IValidateObjectState<Problem>, WorkflowEntityStateValidator<Problem>>()
                .AddScoped<IValidateObjectState<WorkOrder>, WorkflowEntityStateValidator<WorkOrder>>()
                .AddScoped<IValidateObjectState<ChangeRequest>, WorkflowEntityStateValidator<ChangeRequest>>()
                .AddScoped<IValidateObjectState<MassIncident>, WorkflowEntityStateValidator<MassIncident>>();

            services.AddListViews();
            

            // Negotiation Status calculation
            services.AddMappingScoped(
                new ServiceMapping<NegotiationMode, ICalculateNegotiationStatus>()
                    .Map<VoteAllCalculator>().To(NegotiationMode.VoteAll)
                    .Map<FirstAgainstCalculator>().To(NegotiationMode.FirstVoteAgainst)
                    .Map<FirstForCalculator>().To(NegotiationMode.FirstVoteFor)
                    .Map<FirstVoteCalculator>().To(NegotiationMode.FirstVoteAny));


            //  Notes
            //  ObjectNoteQuery
            services.AddScoped(typeof(INotesBLL<>),  typeof(NotesBLL<>));

            // Dependencies
            services.AddScoped(
                typeof(IDependencyBLL<>),
                typeof(DependencyBLL<>));

            services.AddEvents();

            services.AddMappingScoped(
                new ServiceMapping<ObjectClass, ICallReferenceBLL>()
                    .Map<CallReferenceBLL<ChangeRequest>>().To(ObjectClass.ChangeRequest)
                    .Map<CallReferenceBLL<Problem>>().To(ObjectClass.Problem));

            // Object permission validators (overriding defaults)
            services
                .AddServiceDeskObjectPermissionValidator<ChangeRequest>()
                .AddServiceDeskObjectPermissionValidator<WorkOrder>();

            // Согласования
            services.AddMappingScoped(
                new ServiceMapping<ObjectClass, IEditNegotiationBLL>()
                    .Map<LegacyNegotiationBLL<Call>>().To(ObjectClass.Call)
                    .Map<LegacyNegotiationBLL<ChangeRequest>>().To(ObjectClass.ChangeRequest)
                    .Map<LegacyNegotiationBLL<Problem>>().To(ObjectClass.Problem)
                    .Map<LegacyNegotiationBLL<WorkOrder>>().To(ObjectClass.WorkOrder)
                    .Map<MassIncidentNegotiationBLL>().To(ObjectClass.MassIncident));

            services.AddMappingScoped(
                new ServiceMapping<ObjectClass, IObjectSummaryBLL>()
                    .Map<ObjectSummaryBLL<Call>>().To(ObjectClass.Call)
                    .Map<ObjectSummaryBLL<ChangeRequest>>().To(ObjectClass.ChangeRequest)
                    .Map<ObjectSummaryBLL<Problem>>().To(ObjectClass.Problem)
                    .Map<ObjectSummaryBLL<WorkOrder>>().To(ObjectClass.WorkOrder)
                    .Map<ObjectSummaryBLL<MassIncident>>().To(ObjectClass.MassIncident));
            services.AddEntityVisitors();

            services
                .AddScoped<
                    IFindExecutorBLL<UserListItem, UserListFilter>,
                    FindExecutorBLL<UserListItem, UserExecutorListQueryResultItem, UserListFilter, User>>()
                .AddScoped<
                    IFindExecutorBLL<GroupDetails, GroupFilter>,
                    FindExecutorBLL<GroupDetails, GroupExecutorListQueryResultItem, GroupFilter, Group>>();

            services.AddMappingScoped(
                new ServiceMapping<ObjectClass, IModifyWorkOrderExecutorControl>()
                    .Map<WorkOrderExecutorControlModifier<Problem>>().To(ObjectClass.Problem)
                    .Map<WorkOrderExecutorControlModifier<Call>>().To(ObjectClass.Call));

            return services;
        }

        #endregion

        #region Reports

        private static IServiceCollection AddListViews(this IServiceCollection services)
        {
            // Add ListViews
            // TODO: Регистрацию простых списков нужно сделать автоматической (как и было до рефакторинга)
            // All Calls
            services
                .AddTransient<
                    IBuildListViewFilterPredicates<Call, ServiceDeskListFilter>,
                    ServiceDeskListFilterPredicatesBuilder<Call>>()
                .AddSimpleUserSpecification<Call, CallListItem, IBuildAccessIsGrantedSpecification<Call>>() // фильтрует список "Все заявки" по func_accessIsGranted
                .AddListView<CallListItem, ServiceDeskListFilter>()
                    .AsQuery<Call, AllCallsQueryResultItem>();

            // Problems
            services
                .AddTransient<
                    IBuildListViewFilterPredicates<Problem, ServiceDeskListFilter>,
                    ServiceDeskListFilterPredicatesBuilder<Problem>>()
                .AddSimpleUserSpecification<Problem, ProblemListItem, AllProblemsReportUserSpecificationBuilder>() // фильтрует список "Проблемы" по func_accessIsGranted
                .AddListView<ProblemListItem, ServiceDeskListFilter>()
                    .AsQuery<Problem, ProblemListQueryResultItem>();

            // Work Orders
            services
                .AddTransient<
                    IBuildListViewFilterPredicates<WorkOrder, ServiceDeskListFilter>,
                    ServiceDeskListFilterPredicatesBuilder<WorkOrder>>()
                .AddSimpleUserSpecification<WorkOrder, WorkOrderListItem, IBuildAccessIsGrantedSpecification<WorkOrder>>() // фильтрует список "Все задачи" и "Инвентаризация" по func_accessIsGranted
                .AddListView<WorkOrderListItem, ServiceDeskListFilter>()
                    .AsQuery<WorkOrder, WorkOrderListQueryResultItem>()
                .AddListView<InventoryListItem, ServiceDeskListFilter>()
                    .AsQuery<WorkOrder, InventoryListQueryResultItem>();

            services
                .AddTransient<
                    IBuildListViewFilterPredicates<WorkOrder, WorkOrderListFilter>,
                    ServiceDeskWorkOrderFilterPredicateBuilder>()
                .AddListView<ReferencedWorkOrderListItem, WorkOrderListFilter>()
                    .AsQuery<WorkOrder, WorkOrderListQueryResultItem>();

            // All RFCs
            services
                .AddTransient<
                    IBuildListViewFilterPredicates<ChangeRequest, ServiceDeskListFilter>,
                    ServiceDeskListFilterPredicatesBuilder<ChangeRequest>>()
                .AddSimpleUserSpecification<ChangeRequest, ChangeRequestListItem, IBuildAccessIsGrantedSpecification<ChangeRequest>>() // фильтрует список "Запросы на изменения" по func_accessIsGranted
                .AddListView<ChangeRequestListItem, ServiceDeskListFilter>()
                    .AsQuery<ChangeRequest, ChangeRequestQueryResultItem>();

            //На согласовании
            services
                .AddTransient<
                    IBuildListViewFilterPredicates<Call, NegotiationListFilter>,
                    NegotiationListFilterBuilder<Call>>()
                .AddTransient<
                    IBuildListViewFilterPredicates<Problem, NegotiationListFilter>,
                    NegotiationListFilterBuilder<Problem>>()
                .AddTransient<
                    IBuildListViewFilterPredicates<WorkOrder, NegotiationListFilter>,
                    NegotiationListFilterBuilder<WorkOrder>>()
                .AddTransient<
                    IBuildListViewFilterPredicates<ChangeRequest, NegotiationListFilter>,
                    NegotiationListFilterBuilder<ChangeRequest>>()
                .AddTransient<
                    IBuildListViewFilterPredicates<MassIncident, NegotiationListFilter>,
                    NegotiationListFilterBuilder<MassIncident>>()
                .AddListView<NegotiationListItem, NegotiationListFilter>()
                    .AsJoin<
                        Negotiation,
                        NegotiationListQueryResultItem,
                        NegotiationListSubQueryResultItem>(
                    x => x.Union(
                        u => u.Query<Call>(),
                        u => u.Query<WorkOrder>(),
                        u => u.Query<Problem>(),
                        u => u.Query<ChangeRequest>(),
                        u => u.Query<MassIncident>()));

            //Мои задачи
            services
                .AddSimpleUserSpecification<Call, MyTasksReportItem, IBuildAccessIsGrantedSpecification<Call>>()
                .AddSimpleUserSpecification<Problem, MyTasksReportItem, AllProblemsReportUserSpecificationBuilder>()
                .AddSimpleUserSpecification<WorkOrder, MyTasksReportItem, IBuildAccessIsGrantedSpecification<WorkOrder>>()
                .AddSimpleUserSpecification<MassIncident, MyTasksReportItem, AllMassIncidentsReportUserSpecificationBuilder>()
                .AddListView<MyTasksReportItem, ServiceDeskListFilter>()
                    .AsUnion<MyTasksListQueryResultItem>(
                        u => u.Query<Call>(),
                        u => u.Query<WorkOrder>(),
                        u => u.Query<Problem>(),
                        u => u.Query<MassIncident>());

            //CallFromMe
            services
                .AddTransient<
                    IBuildListViewFilterPredicates<Call, CallFromMeListFilter>,
                    CallFromMeListFilterPredicatesBuilder<Call>>()
                .AddListView<CallFromMeListItem, CallFromMeListFilter>()
                    .AsQuery<Call, CallsFromMeListQueryResultItem>();

            //Under control
            services
                .AddListView<ObjectUnderControl, ServiceDeskListFilter>()
                    .AsUnion<ObjectUnderControlQueryResultItem>(
                        u => services.UnderControlQuery<Call>(u),
                        u => services.UnderControlQuery<ChangeRequest>(u),
                        u => services.UnderControlQuery<Problem>(u),
                        u => services.UnderControlQuery<WorkOrder>(u),
                        u => services.UnderControlQuery<MassIncident>(u));

            //All Mass incidents
            services
                .AddTransient<
                    IBuildListViewFilterPredicates<MassIncident, ServiceDeskListFilter>,
                    ServiceDeskListFilterPredicatesBuilder<MassIncident>>()
                .AddSimpleUserSpecification<MassIncident, AllMassIncidentsReportItem, AllMassIncidentsReportUserSpecificationBuilder>()
                .AddListView<AllMassIncidentsReportItem, ServiceDeskListFilter>()
                .AsQuery<MassIncident, AllMassIncidentsReportQueryResultItem>();

            // MassIncidentsToAssociate
            services
                .AddScoped<
                    IAggregatePredicateBuilders<MassIncident, MassIncidentsToAssociateReportItem>,
                    MassIncidentToReportItemPredicateBuilder<MassIncidentsToAssociateReportItem>>()
                .AddScoped<
                    IAggregatePredicateBuilders<MassIncidentsReportQueryResultItem, MassIncidentsToAssociateReportItem>,
                    MassIncidentsReportPredicateBuilder<MassIncidentsToAssociateReportItem>>()
                .AddScoped<
                    IBuildListViewFilterPredicates<MassIncident, MassIncidentsToAssociateFilter>,
                    MassIncidentsToAssociateFilterPredicatesBuilder>()
                .AddListView<MassIncidentsToAssociateReportItem, MassIncidentsToAssociateFilter>()
                .AsQuery<MassIncident, MassIncidentsReportQueryResultItem>();

            // ProblemMassIncidents
            services
                .AddScoped<
                    IAggregatePredicateBuilders<MassIncident, ProblemMassIncidentsReportItem>,
                    MassIncidentToReportItemPredicateBuilder<ProblemMassIncidentsReportItem>>()
                .AddScoped<
                    IAggregatePredicateBuilders<MassIncidentsReportQueryResultItem, ProblemMassIncidentsReportItem>,
                    MassIncidentsReportPredicateBuilder<ProblemMassIncidentsReportItem>>()
                .AddTransient<
                    IBuildListViewFilterPredicates<MassIncident, ProblemMassIncidentFilter>,
                    ProblemMassIncidentFilterPredicatesBuilder>()
                .AddListView<ProblemMassIncidentsReportItem, ProblemMassIncidentFilter>()
                .AsQuery<MassIncident, MassIncidentsReportQueryResultItem>();


            return services;
        }

        private static void UnderControlQuery<T>(
            this IServiceCollection services,
            ISubQueryConfigurer<ObjectUnderControl, ServiceDeskListFilter, ObjectUnderControlQueryResultItem> configurer)
            where T : IGloballyIdentifiedEntity
        {
            services.AddScoped<IListViewUserSpecification<T, ObjectUnderControl>, ObjectsUnderControlReportSpecificationBuilder<T>>();
            configurer.Query<T>();
        }

        #endregion

        #region Entity Visitors

        private static IServiceCollection AddEntityVisitors(this IServiceCollection services)
        {
            // EntityEvent creators
            services.AddScoped<IVisitNewEntity<Call>, WorkflowEntityCreatedEventVisitor<Call>>();
            services.AddScoped<IVisitNewEntity<Call>, WorkflowSetStateEventVisitor<Call>>();
            services.AddScoped<IVisitModifiedEntity<Call>, WorkflowSetStateEventVisitor<Call>>();
            services.AddScoped<IVisitModifiedEntity<Call>, WorkflowEntityModifiedEventVisitor<Call>>();
            services.AddScoped<IVisitNewEntity<Problem>, WorkflowEntityCreatedEventVisitor<Problem>>();
            services.AddScoped<IVisitNewEntity<Problem>, WorkflowSetStateEventVisitor<Problem>>();
            services.AddScoped<IVisitModifiedEntity<Problem>, WorkflowSetStateEventVisitor<Problem>>();
            services.AddScoped<IVisitModifiedEntity<Problem>, WorkflowEntityModifiedEventVisitor<Problem>>();
            services.AddScoped<IVisitNewEntity<WorkOrder>, WorkflowSetStateEventVisitor<WorkOrder>>();
            services.AddScoped<IVisitModifiedEntity<WorkOrder>, WorkflowSetStateEventVisitor<WorkOrder>>();
            services.AddScoped<IVisitModifiedEntity<WorkOrder>, WorkflowEntityModifiedEventVisitor<WorkOrder>>();
            services.AddScoped<IVisitModifiedEntity<ChangeRequest>, WorkflowEntityModifiedEventVisitor<ChangeRequest>>();
            services.AddScoped<IVisitNewEntity<ChangeRequest>, WorkflowSetStateEventVisitor<ChangeRequest>>();
            services.AddScoped<IVisitModifiedEntity<ChangeRequest>, WorkflowSetStateEventVisitor<ChangeRequest>>();
            services.AddScoped<IVisitNewEntity<ChangeRequest>, WorkflowEntityCreatedEventVisitor<ChangeRequest>>();
            services.AddScoped<IVisitNewEntity<MessageByEmail>, WorkflowEntityCreatedEventVisitor<MessageByEmail>>();
            services.AddScoped<IVisitNewEntity<MessageByEmail>, WorkflowSetStateEventVisitor<MessageByEmail>>();
            services.AddScoped<IVisitModifiedEntity<MessageByEmail>, WorkflowSetStateEventVisitor<MessageByEmail>>();
            services.AddScoped<IVisitModifiedEntity<MessageByEmail>, WorkflowEntityModifiedEventVisitor<MessageByEmail>>();
            services.AddScoped<IVisitNewEntity<MassIncident>, WorkflowSetStateEventVisitor<MassIncident>>();
            services.AddScoped<IVisitModifiedEntity<MassIncident>, WorkflowSetStateEventVisitor<MassIncident>>();
            services.AddScoped<IVisitModifiedEntity<MassIncident>, WorkflowEntityModifiedEventVisitor<MassIncident>>();

            // Note<T> visitors
            services
                .AddDefaultNoteVisitor<Call>()
                .AddDefaultNoteVisitor<WorkOrder>()
                .AddDefaultNoteVisitor<Problem>()
                .AddDefaultNoteVisitor<MassIncident>()
                .AddDefaultNoteVisitor<ChangeRequest>();

            return services
                 .AddCallReferenceVisitor<Problem>()
                 .AddCallReferenceVisitor<ChangeRequest>();
        }

        private static IServiceCollection AddCallReferenceVisitor<TReference>(this IServiceCollection services)
            where TReference : IHaveUtcModifiedDate, IGloballyIdentifiedEntity
        {
            services.AddScoped<IVisitNewEntity<CallReference<TReference>>, CallReferenceVisitor<TReference>>();
            services.AddScoped<IVisitDeletedEntity<CallReference<TReference>>, CallReferenceVisitor<TReference>>();
            return services;
        }

        private static IServiceCollection AddDefaultNoteVisitor<T>(this IServiceCollection services)
            where T : class, IHaveUtcModifiedDate, IGloballyIdentifiedEntity
        {
            return services.AddScoped<IVisitNewEntity<Note<T>>, DefaultNoteVisitor<T>>();
        }

        #endregion

        private static IServiceCollection AddServiceDeskObjectPermissionValidator<T>(this IServiceCollection services)
            where T : class, IGloballyIdentifiedEntity
        {
            return services.AddScoped<IValidateObjectPermissions<Guid, T>, ServiceDeskAccessValidator<T>>();
        }

        private static IServiceCollection AddEvents(this IServiceCollection services)
        {
            return services
                .AddCallEvents()
                .AddWorkOrderEvents()
                .AddProblemEvents()
                .AddChangeRequestEvents()
                .AddNegotiationEvents()
                .AddKnowledgeBaseArticleEvents()
                .AddManhoursEvents()
                .AddELPEvents()
                .AddMassIncidentEvents()
                .AddOLAEvents()
                .AddUserEvents();
        }
    }
}
