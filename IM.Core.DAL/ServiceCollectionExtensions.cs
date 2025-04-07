using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Asset.History;
using InfraManager.DAL.AssetsManagement;
using InfraManager.DAL.Asset.History;
using InfraManager.DAL.AssetsManagement.Hardware;
using InfraManager.DAL.DeleteStrategies;
using InfraManager.DAL.Location;
using InfraManager.DAL.MaintenanceWork;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using InfraManager.DAL.ProductCatalogue.ProductCatalogModel;
using InfraManager.DAL.ServiceCatalog.PortfolioService;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using InfraManager.DAL.ServiceCatalogue.SLA;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Calls;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.Manhours;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.Negotiations;
using InfraManager.DAL.ServiceDesk.Problems;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using InfraManager.DAL.Software;
using InfraManager.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.DAL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDAL(this IServiceCollection services)
        {
            // Add DbContext included all related stuff
            services.AddDbContext<CrossPlatformDbContext>();
            services.AddScoped<IUnitOfWork>(provider => provider.GetService<CrossPlatformDbContext>());
            // Add DbSet for each class in entities
            services.AddDbSets();

            // Register default implemetations

            services.AddScoped(
                typeof(IFinder<>),
                typeof(Finder<>));
            services.AddDefaultGenericImplementationScoped(
                EntityTypes.Where(t => t.HasInterface<IGloballyIdentifiedEntity>()),
                typeof(IFindEntityByGlobalIdentifier<>),
                typeof(GlobalFinder<>));
            // Add default repositories 
            services.AddScoped(
                typeof(IRepository<>),
                typeof(Repository<>));
            services.AddScoped(
                typeof(IReadonlyRepository<>),
                typeof(Repository<>));
            services
                .AddScoped(typeof(IDeleteStrategy<>), typeof(PhysicalDeleteStrategy<>))
                .RemoveIconCascade<MassIncidentType>(); //TODO: Вернуться к обсуждению регистрации стратегии удаления иконок
            services.AddDefaultGenericImplementationScoped(
                EntityTypes.Where(t => t.HasInterface<IMarkableForDelete>()),
                typeof(IDeleteStrategy<>),
                typeof(LogicalDeleteStrategy<>));
            services.AddScoped(typeof(ICatalogFinder<,>), typeof(CatalogFinder<,>));
            services.AddScoped(typeof(IListQuery<,>), typeof(ListQuery<,>));
            services.AddScoped(typeof(IWorkOrderListQueryBase<>), typeof(WorkOrderListQueryBase<>));

            //  ObjectNoteQuery
            services.AddScoped(
                typeof(IObjectNoteQuery<>),
                typeof(ObjectNoteQuery<>));

            services.AddScoped(typeof(INoteQuery<>), typeof(NoteQuery<>));
            services.AddScoped(typeof(NoteCountExpressionCreator<>));
            services.AddScoped(typeof(IBuildObjectIsUnderControlSpecification<>), typeof(ObjectIsUnderControlSpecificationBuilder<>));

            // Add all services marked as ISelfRegisteredService
            var thisAssembly = Assembly.GetExecutingAssembly();            
            services.AddSelfRegisteredServices(thisAssembly);
            
            // Add Data Providers (опасная практика лучше использовать более конкретные IRepository, IFinder и т.д.)           
            services.AddScoped<ISoftwareModelDataProvider, SoftwateModelDataProvider>();

            services.AddObjectClassMapping<IFindName>(typeof(NameFinder<>), typeof(INamedEntity));
            services.AddObjectClassMapping<IObjectHistoryNameGetter>(typeof(ObjectHistoryNameGetter<>), typeof(IHistoryNamedEntity));
            services.AddObjectClassMapping<ILocationFullPathGetter>(typeof(LocationFullPathGetter<>), typeof(ILocationObject));
            services.AddObjectClassMapping<IFindNameByGlobalID>(
                typeof(GlobalIdentifierNameFinder<>),
                typeof(INamedEntity),
                typeof(IGloballyIdentifiedEntity));
            services.AddObjectClassMapping<IAnyQuery>(typeof(AnyQuery<>), typeof(IGloballyIdentifiedEntity));
            services.AddObjectClassMapping<IFindEntityWithManhours>(
                typeof(EntityWithManhoursFinder<>),
                typeof(IHaveManhours));
            services.AddAbstractFinderToObjectClassMapping<ICreateWorkOrderReference, Guid>();

            services.AddObjectClassMapping<IFindEntityByGlobalIdentifier>(
                typeof(GlobalFinder<>),
                typeof(IGloballyIdentifiedEntity));

            services.AddMappingScoped(
                new ServiceMapping<ObjectClass, IMaintenanceNodeTreeQuery>()
                    .Map<MaintenanceFolderTreeQuery>().To(ObjectClass.MaintenanceFolder)
                    .Map<MaintenanceTreeQuery>().To(ObjectClass.Maintenance));

            services.AddMappingScoped(
                new ServiceMapping<ObjectClass, IQueryCallClientLocationInfo>()
                    .Map<WorkplaceCallClientLocationInfoQuery>().To(ObjectClass.Workplace)
                    .Map<RoomCallClientLocationInfoQuery>().To(ObjectClass.Room));

            services.AddScoped(typeof(IObjectSummaryInfoQuery<>), typeof(ObjectSummaryQuery<>));

            services.AddMappingScoped(
                new ServiceMapping<ObjectClass, ILifeCycleNodeQuery>()
                    .Map<LifeCycleNodeQuery>().To(ObjectClass.LifeCycle)
                    .Map<LifeCycleStateNodeQuery>().To(ObjectClass.LifeCycleState)
                    .Map<LifeCycleStateOperationNodeQuery>().To(ObjectClass.Unknown));

            services.AddMappingScoped(
                new ServiceMapping<ObjectClass, ILocationNodesQuery>()
                    .Map<LocationNodeFloorQuery>().To(ObjectClass.Floor)
                    .Map<LocationNodeRoomQuery>().To(ObjectClass.Room)
                    .Map<LocationNodeWorkplaceQuery>().To(ObjectClass.WorkOrder)
                    .Map<LocationNodeRackQuery>().To(ObjectClass.Rack));

            services.AddMappingScoped(new ServiceMapping<ObjectClass, IPortfolioServiceTreeQuery>()
                .Map<ServiceCategoryTreeQuery>().To(ObjectClass.ServiceCategory)
                .Map<ServiceItemOrAttendanceTreeQuery>().To(ObjectClass.ServiceItem)
                .Map<ServiceItemOrAttendanceTreeQuery>().To(ObjectClass.ServiceAttendance)
                .Map<ServiceTreeQuery>().To(ObjectClass.Service));

            services.AddObjectAccessQueries();

            //Add Paging Query creator
            services.AddSingleton<IPagingQueryCreator, PagingQueryCreator>();

            // Add Software Installation List Queries mapper
            services.AddObjectClassServiceMapperScoped<Software.Installation.ISoftwareInstalationListQuery>(thisAssembly);

            // Add Queryable (TODO: auto register IQueryables)
            services.AddQueryableScoped<GroupUser>();

            services
                .AddScoped<IFinder<TerminalDeviceModel>, ProductCatalogModelFinder<TerminalDeviceModel>>()
                .AddScoped<IFinder<NetworkDeviceModel>, ProductCatalogModelFinder<NetworkDeviceModel>>()
                .AddScoped<IFinder<CabinetType>, ProductCatalogModelFinder<CabinetType>>();

            services
                .AddScoped(typeof(IBulkDelete<>),typeof(BulkDelete<>));

            services.AddTimeZoneObjects();
            
            services
                .AddScoped<ITaskStateNameQuery, ServiceDeskEntityStateLookupQuery<Call>>()
                .AddScoped<ITaskStateNameQuery, ServiceDeskEntityStateLookupQuery<Problem>>()
                .AddScoped<ITaskStateNameQuery, ServiceDeskEntityStateLookupQuery<WorkOrder>>()
                .AddScoped<ITaskStateNameQuery, ServiceDeskEntityStateLookupQuery<MassIncident>>();
            services
                .AddScoped<IQueryIssueTypes, IssueTypesQuery<Call, CallType>>()
                .AddScoped<IQueryIssueTypes, IssueTypesQuery<MassIncident, MassIncidentType>>()
                .AddScoped<IQueryIssueTypes, IssueTypesQuery<Problem, ProblemType>>()
                .AddScoped<IQueryIssueTypes, IssueTypesQuery<WorkOrder, WorkOrderType>>();

            services.AddScoped(typeof(IBuildAccessIsGrantedSpecification<>), typeof(AccessIsGrantedSpecificationBuilder<>));
            services.AddScoped(typeof(IBuildUserInNegotiationSpecification<>), typeof(UserInNegotiationSpecificationBuilder<>));

            services.AddExecutorQueries();

            services.AddHardwareQuery();

            return services;
        }

        public static ServiceMapping<LookupQueries, ILookupQuery> DataAccessQueries =>
            new ServiceMapping<LookupQueries, ILookupQuery>()
                .Map<ReferencedProblemTypesLookupQuery>().To(LookupQueries.ReferencedProblemTypes)
                .Map<ServiceDeskEntityStateLookupQuery<Problem>>().To(LookupQueries.ProblemStateNames)
                .Map<ProblemUrgenciesLookupQuery>().To(LookupQueries.ProblemUrgencies)
                .Map<ProblemInfluencesLookupQuery>().To(LookupQueries.ProblemInfluences)
                .Map<ProblemPrioritiesLookupQuery>().To(LookupQueries.ProblemPriorities)
                .Map<ProblemBudgetLookupQuery>().To(LookupQueries.ProblemBudget)
                .Map<ProblemBudgetUsageCauseLookupQuery>().To(LookupQueries.ProblemBudgetUsageCause)
                .Map<CallReceiptTypeLookupQuery>().To(LookupQueries.CallReceiptType)
                .Map<CallSlaLookupQuery>().To(LookupQueries.CallSla)
                .Map<CallServiceNameLookupQuery>().To(LookupQueries.CallServiceName)
                .Map<CallUrgenciesLookupQuery>().To(LookupQueries.CallUrgencies)
                .Map<CallInfluencesLookupQuery>().To(LookupQueries.CallInfluences)
                .Map<CallPrioritiesLookupQuery>().To(LookupQueries.CallPriorities)
                .Map<ServiceDeskEntityStateLookupQuery<Call>>().To(LookupQueries.CallStateName)
                .Map<CallBudgetLookupQuery>().To(LookupQueries.CallBudget)
                .Map<CallBudgetUsageCauseLookupQuery>().To(LookupQueries.CallBudgetUsageCause)
                .Map<CallTypeLookupQuery>().To(LookupQueries.CallType)
                .Map<WorkOrderBudgetLookupQuery>().To(LookupQueries.WorkOrderBudget)
                .Map<WorkOrderBudgetUsageCauseLookupQuery>().To(LookupQueries.WorkOrderBudgetUsageCause)
                .Map<WorkOrderPrioritiesLookupQuery>().To(LookupQueries.WorkOrderPriorities)
                .Map<ServiceDeskEntityStateLookupQuery<WorkOrder>>().To(LookupQueries.WorkOrderStateName)
                .Map<WorkOrderTypeLookupQuery>().To(LookupQueries.WorkOrderType)              
                .Map<MyTasksPriorityLookupQuery>().To(LookupQueries.TaskPriority)
                .Map<MyTasksStateNameLookupQuery>().To(LookupQueries.TaskStateName)                
                .Map<MyTasksTypeLookupQuery>().To(LookupQueries.TaskType)
                .Map<ChangeRequestCategoryLookupQuery>().To(LookupQueries.ChangeRequestCategory)
                .Map<ChangeRequestInfluenceLookupQuery>().To(LookupQueries.ChangeRequestInfluence)
                .Map<ChangeRequestPriorityLookupQuery>().To(LookupQueries.ChangeRequestPriority)
                .Map<ChangeRequestStateNameLookupQuery>().To(LookupQueries.ChangeRequestStateName)
                .Map<ChangeRequestTypeLookupQuery>().To(LookupQueries.ChangeRequestType)
                .Map<ChangeRequestUrgencyLookupQuery>().To(LookupQueries.ChangeRequestUrgency)
                .Map<MassIncidentTypeLookupQuery>().To(LookupQueries.MassIncidentTypes)
                .Map<MassIncidentCauseLookupQuery>().To(LookupQueries.MassIncidentCauses)
                .Map<MassincidentCriticalitiesLookupQuery>().To(LookupQueries.MassIncidentCriticalities)
                .Map<MassIncidentPrioritiesLookupQuery>().To(LookupQueries.MassIncidentPriorities)
                .Map<MassIncidentServicesLookupQuery>().To(LookupQueries.MassIncidentServices)
                .Map<MassIncidentStatesLookupQuery>().To(LookupQueries.MassIncidentStates)
                .Map<MassIncidentOLALookupQuery>().To(LookupQueries.MassIncidentSLAs)
                .Map<SupplierLookupQuery>().To(LookupQueries.Suppliers)
                .Map<ManufacturerLookupQuery>().To(LookupQueries.Manufacturers)
                .Map<ProductCatalogTypeLookupQuery>().To(LookupQueries.ProductCatalogTypes)
                .Map<ProductCatalogTemplateLookupQuery>().To(LookupQueries.ProductCatalogTemplates)
                .Map<ServiceContractLookupQuery>().To(LookupQueries.ServiceContracts)
                .Map<ServiceContractLifeCycleStateLookupQuery>().To(LookupQueries.ServiceContractStates)
                .Map<HardwareStateLookupQuery>().To(LookupQueries.HardwareStates)
                .Map<HardwareModelNameLookupQuery>().To(LookupQueries.HardwareModelNames)
                .Map<ProblemServiceNameLookupQuery>().To(LookupQueries.ProblemServiceName);

        private static IServiceCollection AddObjectAccessQueries(this IServiceCollection services)
        {
            var mapping = new ServiceMapping<ObjectClass, IObjectAccessQuery>();

            foreach(var entityType in EntityTypes
                .Where(t => t.HasAttribute<ObjectClassMappingAttribute>())
                .Where(t => t.HasInterface<IGloballyIdentifiedEntity>()))
            {
                var objectMapping = entityType.GetAttribute<ObjectClassMappingAttribute>();
                var queryType = typeof(ObjectAccessQuery<>).MakeGenericType(entityType);
                var registrationMethod =
                    typeof(ServiceCollectionExtensions)
                        .GetMethod(nameof(AddObjectAccessQueryScoped))
                        .MakeGenericMethod(entityType);
                registrationMethod.Invoke(obj: null, new object[] { services, objectMapping.ObjectClass });
                mapping.Map(queryType).To(objectMapping.ObjectClass);
            }

            return services.AddMappingScoped(mapping);
        }
        
        private static IServiceCollection AddTimeZoneObjects(this IServiceCollection services)
        {
            var timeZoneObjects = EntityTypes.Where(t => t.HasInterface<ITimeZoneObject>()).ToArray();
            foreach (var entityType in timeZoneObjects)
            {
                services.AddScoped(typeof(ITimeZoneObjects),typeof(TimeZoneObjects<>).MakeGenericType(entityType));
            }
            
            return services;
        }


        public static IServiceCollection AddObjectAccessQueryScoped<T>(
            this IServiceCollection services, 
            ObjectClass ownerClass) where T : class, IGloballyIdentifiedEntity
        {
            return services.AddScoped(
                provider =>
                    new ObjectAccessQuery<T>(provider.GetService<DbSet<T>>(), ownerClass));
        }

        private static IEnumerable<Type> EntityTypes => typeof(Lookup).Assembly.GetTypes() // все типы сборки Entities
                .Where(t => t.IsClass);

        private static IServiceCollection AddDbSets(this IServiceCollection services)
        {
            var getDbSet = typeof(DbContext).GetMethod(nameof(DbContext.Set), Array.Empty<Type>());

            foreach (var entityType in EntityTypes)
            {
                var genericSet = getDbSet.MakeGenericMethod(entityType);
                services.AddScoped(
                    typeof(DbSet<>).MakeGenericType(entityType),
                    provider =>
                        genericSet.Invoke(provider.GetService<CrossPlatformDbContext>(), Array.Empty<object>()));
            }
            
            return services
                .AddManyToManyDbSet<OperationalLevelAgreement, Service>()
                .AddManyToManyDbSet<MassIncident, Call>()
                .AddManyToManyDbSet<MassIncident, ChangeRequest>()
                .AddManyToManyDbSet<MassIncident, Problem>()
                .AddManyToManyDbSet<MassIncident, WorkOrder>()
                .AddManyToManyDbSet<MassIncident, Service>()
                .AddCallReferenceDbSet<ChangeRequest>()
                .AddCallReferenceDbSet<Problem>()                
                .AddNoteDbSet<Call>()
                .AddNoteDbSet<WorkOrder>()
                .AddNoteDbSet<Problem>()
                .AddNoteDbSet<ChangeRequest>()
                .AddNoteDbSet<MassIncident>();
        }

        private static IServiceCollection AddNoteDbSet<T>(this IServiceCollection services) =>
            services.AddScoped(provider => provider.GetService<CrossPlatformDbContext>().Set<Note<T>>());

        private static IServiceCollection AddManyToManyDbSet<TParent, TReference>(this IServiceCollection services)
            where TParent : class
            where TReference : class
        {
            return services.AddScoped(
                serviceProvider => serviceProvider
                    .GetService<CrossPlatformDbContext>()
                    .Set<ManyToMany<TParent, TReference>>());
        }
        
        private static IServiceCollection AddCallReferenceDbSet<T>(this IServiceCollection services) where T: IHaveUtcModifiedDate, IGloballyIdentifiedEntity =>
            services.AddScoped(provider => provider.GetService<CrossPlatformDbContext>().Set<CallReference<T>>());

        private static IServiceCollection AddObjectClassMapping<T>(
            this IServiceCollection services,
            Type implementation,
            params Type[] requiredInterfaces) where T : class
        {
            var serviceMapping = new ServiceMapping<ObjectClass, T>();

            foreach (var entityType in EntityTypes
                .Where(tp => requiredInterfaces.All(i => tp.IsAssignableTo(i)))
                .Where(tp => tp.HasAttribute<ObjectClassMappingAttribute>()))
            {
                serviceMapping.Map(implementation.MakeGenericType(entityType))
                    .To(entityType.GetAttribute<ObjectClassMappingAttribute>().ObjectClass);
            }

            return services.AddMappingScoped(serviceMapping);
        }

        private static IServiceCollection AddQueryableScoped<T>(this IServiceCollection services)
            where T : class
        {
            return services.AddScoped(provider => provider.GetService<DbSet<T>>().AsQueryable());
        }

        private static IServiceCollection AddAbstractFinderToObjectClassMapping<TAbstract, TKey>(this IServiceCollection services)
        {
            var serviceMapping = new ServiceMapping<ObjectClass, IAbstractFinder<TAbstract, TKey>>();

            foreach (var entityType in EntityTypes
                .Where(tp => tp.IsAssignableTo(typeof(TAbstract)))
                .Where(tp => tp.HasAttribute<ObjectClassMappingAttribute>()))
            {
                serviceMapping.Map(typeof(AbstractFinder<,,>).MakeGenericType(entityType, typeof(TAbstract), typeof(TKey)))
                    .To(entityType.GetAttribute<ObjectClassMappingAttribute>().ObjectClass);
            }

            return services.AddMappingScoped(serviceMapping);
        }

        private static IServiceCollection RemoveIconCascade<T>(this IServiceCollection services)
            where T : class, IGloballyIdentifiedEntity
        {
            return services.AddScoped<IDependentDeleteStrategy<T>, ObjectIconDepencyDeleteStrategy<T>>();
        }
        
        private static IServiceCollection AddExecutorQueries(this IServiceCollection services)
        {
            services
                .AddScoped<IBuildAvailableToExecutorViaTtz<User, WorkOrder>, EntityIsAvailableToExecutorViaTtz<WorkOrder, User, WorkorderDependency>>()
                .AddScoped<IBuildAvailableToExecutorViaToz<User, WorkOrder>, WorkOrderIsAvailableToExecutorViaToz>()
                .AddScoped<IBuildAvailableToExecutorViaSupportLine<User, WorkOrder>, WorkOrderIsAvailableToExecutorViaSupportLine>();

            services
                .AddScoped<IBuildAvailableToExecutorViaTtz<Group, WorkOrder>, EntityIsAvailableToExecutorViaTtz<WorkOrder, Group, WorkorderDependency>>()
                .AddScoped<IBuildAvailableToExecutorViaToz<Group, WorkOrder>, WorkOrderIsAvailableToExecutorViaToz>()
                .AddScoped<IBuildAvailableToExecutorViaSupportLine<Group, WorkOrder>, WorkOrderIsAvailableToExecutorViaSupportLine>();

            services
                .AddScoped<IBuildAvailableToExecutorViaTtz<User, Problem>, EntityIsAvailableToExecutorViaTtz<Problem, User, ProblemDependency>>()
                .AddScoped<IBuildAvailableToExecutorViaToz<User, Problem>, DefaultIsAvailableToExecutor<Problem>>()
                .AddScoped<IBuildAvailableToExecutorViaSupportLine<User, Problem>, DefaultIsAvailableToExecutor<Problem>>();

            services
                .AddScoped<IBuildAvailableToExecutorViaTtz<Group, Problem>, EntityIsAvailableToExecutorViaTtz<Problem, Group, ProblemDependency>>()
                .AddScoped<IBuildAvailableToExecutorViaToz<Group, Problem>, DefaultIsAvailableToExecutor<Problem>>()
                .AddScoped<IBuildAvailableToExecutorViaSupportLine<Group, Problem>, DefaultIsAvailableToExecutor<Problem>>();

            services
                .AddScoped<IBuildAvailableToExecutorViaTtz<User, Call>, EntityIsAvailableToExecutorViaTtz<Call, User, CallDependency>>()
                .AddScoped<IBuildAvailableToExecutorViaToz<User, Call>, CallIsAvailableToExecutorViaToz>()
                .AddScoped<IBuildAvailableToExecutorViaSupportLine<User, Call>, CallIsAvailableToExecutorViaSupportLine>();

            services
                .AddScoped<IBuildAvailableToExecutorViaTtz<Group, Call>, EntityIsAvailableToExecutorViaTtz<Call, Group, CallDependency>>()
                .AddScoped<IBuildAvailableToExecutorViaToz<Group, Call>, CallIsAvailableToExecutorViaToz>()
                .AddScoped<IBuildAvailableToExecutorViaSupportLine<Group, Call>, CallIsAvailableToExecutorViaSupportLine>();

            services.AddMappingScoped(
                new ServiceMapping<ObjectClass, IExecutorListQuery<UserExecutorListQueryResultItem, User>>()
                    .Map<UserExecutorListQuery<WorkOrder>>().To(ObjectClass.WorkOrder)
                    .Map<UserExecutorListQuery<Problem>>().To(ObjectClass.Problem)
                    .Map<UserExecutorListQuery<Call>>().To(ObjectClass.Call)
            );

            services.AddMappingScoped(
                new ServiceMapping<ObjectClass, IExecutorListQuery<GroupExecutorListQueryResultItem, Group>>()
                    .Map<GroupExecutorListQuery<WorkOrder>>().To(ObjectClass.WorkOrder)
                    .Map<GroupExecutorListQuery<Problem>>().To(ObjectClass.Problem)
                    .Map<GroupExecutorListQuery<Call>>().To(ObjectClass.Call)
            );

            return services;
        }
    }
}
