using Inframanager.BLL;
using Inframanager.BLL.EventsOld;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.Algorithms;
using InfraManager.BLL.Asset;
using InfraManager.BLL.Asset.Adapters;
using InfraManager.BLL.Asset.LifeCycleCommands;
using InfraManager.BLL.Asset.NetworkDevices;
using InfraManager.BLL.Asset.Peripherals;
using InfraManager.BLL.AssetsManagement;
using InfraManager.BLL.AssetsManagement.Hardware;
using InfraManager.BLL.Catalog;
using InfraManager.BLL.Cloners;
using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Localization;
using InfraManager.BLL.Notification;
using InfraManager.BLL.Notification.NotificationProviders;
using InfraManager.BLL.ProductCatalogue;
using InfraManager.BLL.ProductCatalogue.Extensions;
using InfraManager.BLL.ProductCatalogue.ModelCharacteristics;
using InfraManager.BLL.ProductCatalogue.Slots;
using InfraManager.BLL.ProductCatalogue.Tree;
using InfraManager.BLL.Search;
using InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceCategories;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.BLL.ServiceDesk.ChangeRequests;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.BLL.ServiceDesk.Negotiations;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.BLL.Settings;
using InfraManager.BLL.Settings.TableFilters;
using InfraManager.BLL.Settings.TableFilters.FilterElements;
using InfraManager.BLL.Software;
using InfraManager.BLL.Software.SoftwareModels;
using InfraManager.BLL.Users;
using InfraManager.BLL.UserUniqueFiltrations;
using InfraManager.BLL.Workflow;
using InfraManager.BLL.WorkFlow;
using InfraManager.DAL;
using InfraManager.DAL.Accounts;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.Software;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using InfraManager.BLL.ServiceDesk.Search;
using InfraManager.BLL.ServiceDesk.Search.SystemSearch;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using AdapterDetails = InfraManager.BLL.Asset.Adapters.AdapterDetails;
using PeripheralDetails = InfraManager.BLL.Asset.Peripherals.PeripheralDetails;
using InfraManager.BLL.Asset.LifeCycleCommands.DeviceLocation;
using InfraManager.BLL.Asset.History;

namespace InfraManager.BLL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBLL(this IServiceCollection services)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            var interfacesAssemply = typeof(IUserBLL).Assembly;
            services.AddBLLCore(thisAssembly, interfacesAssemply);
            services.ConfigureEventBuilders(thisAssembly);

            // Register defaults
            services.AddScoped(typeof(ILocalizeEnum<>), typeof(StaticResourceEnumLocalizer<>));

            services.AddSelfRegisteredServices(thisAssembly);
            services.AddScoped<IValidateObjectPermissions<int, MassIncident>, MassIncidentAccessValidator>();

            // Add EntityEditor TODO: Автоматически вытянуть все реализации IEntityProvider и смапить на ObjectClass (через атрибут??)
            services.AddObjectClassServiceMapperScoped<FieldEdit.IEntityEditor>(thisAssembly);

            services.AddMemoryCache();
            services.AddSearchers();

            services.AddProductBLL();
            services.AddMappingScoped(
                new ServiceMapping<FilterTypes, ICreateFilterElement>()
                    .Map<DatePickFilterElementCreator>().To(FilterTypes.DatePick)
                    .Map<MultiSelectFilterElementCreator>().To(FilterTypes.SelectorMultiple)
                    .Map<RangleSliderFilterElementCreator>().To(FilterTypes.SliderRange)
                    .Map<SimpleValueFilterElementCreator>().To(FilterTypes.SimpleValue)
                    .Map<PatternFilterElementCreator>().To(FilterTypes.LikeValue)
                    .Map<DefaultFilterElementCreator>().To(FilterTypes.FuncSelectorValue)
                    .Map<DefaultFilterElementCreator>().To(FilterTypes.SelectorValue)
                    .Map<DefaultFilterElementCreator>().To(FilterTypes.SliderValue));

            services.AddMappingScoped(
                new ServiceMapping<HashAlgorithms, IHashAlgorithm>()
                .Map<HashAlgorithm_MD5>().To(HashAlgorithms.MD5)
                .Map<HashAlgorithm_SHA1>().To(HashAlgorithms.SHA_1)
                .Map<HashAlgorithm_SHA224>().To(HashAlgorithms.SHA_224)
                .Map<HashAlgorithm_SHA256>().To(HashAlgorithms.SHA_256)
                .Map<HashAlgorithm_SHA384>().To(HashAlgorithms.SHA_384)
                .Map<HashAlgorithm_SHA512>().To(HashAlgorithms.SHA_512));


            services.AddMappingScoped(
                new ServiceMapping<ObjectClass, INotificationProvider>()
                    .Map<MassiveIncidentNotificationProvider>().To(ObjectClass.MassIncident)
                    .Map<CallNotificationProvider>().To(ObjectClass.Call)
                    .Map<ProblemNotificationProvider>().To(ObjectClass.Problem)
                    .Map<WorkOrderNotificationProvider>().To(ObjectClass.WorkOrder)
                    .Map<NegotiationNotificationProvider>().To(ObjectClass.Negotiation)
                    .Map<CustomControllerNotificationProvider>().To(ObjectClass.CustomController)
                    .Map<SubstitutionNotificationProvider>().To(ObjectClass.Substitution)
                    .Map<ChangeRequestNotificationProvider>().To(ObjectClass.ChangeRequest));

            // TODO: Перейти на StandardBLL
            services
                .AddScoped<
                    ILookupBLL<IncidentResultListItemModel, IncidentResultDetailsModel, IncidentResultModel, Guid>,
                    LookupBLL<IncidentResultListItemModel, IncidentResultDetailsModel, IncidentResultModel, IncidentResult, Guid>>()
                .AddScoped<
                    ILookupBLL<ChangeRequestResultListItem, ChangeRequestResultDetailsModel, ChangeRequestResultModel, Guid>,
                    LookupBLL<ChangeRequestResultListItem, ChangeRequestResultDetailsModel, ChangeRequestResultModel, RequestForServiceResult, Guid>>()
                .AddScoped<
                    ILookupBLL<InfluenceListItemModel, InfluenceDetailsModel, InfluenceModel, Guid>,
                    LookupBLL<InfluenceListItemModel, InfluenceDetailsModel, InfluenceModel, Influence, Guid>>()
                .AddScoped<
                    ILookupBLL<ProblemCauseDetails, ProblemCauseDetails, ProblemCauseData, Guid>,
                    LookupBLL<ProblemCauseDetails, ProblemCauseDetails, ProblemCauseData, ProblemCause, Guid>>()
                 .AddScoped<
                     ILookupBLL< ServiceCategoryItem, ServiceCategoryDetails, ServiceCategoryData, Guid >,
                     LookupBLL <ServiceCategoryItem, ServiceCategoryDetails, ServiceCategoryData, ServiceCategory, Guid>>();


            services.AddServiceDesk();
            services.AddSettings();
            services.AddWorkflow();
            services.AddAssetsManagement();
            services.AddAccessManagement();

            services.AddTransient(typeof(IBasicCatalogBLL<,,,>), typeof(BasicCatalogBLL<,,,>));
            services.AddTransient(typeof(ISupportBLL<>), typeof(SupportBLL<>));
            services.AddTransient(typeof(IPortfolioServiceItemBLL<,>), typeof(PortfolioServiceItemBLL<,>));
            services.AddTransient(typeof(IServiceItemAndAttendanceBLL<,,,>), typeof(ServiceItemAndAttendanceBLL<,,,>));
            services.AddScoped(typeof(IGuidePaggingFacade<,>), typeof(GuidePaggingFacade<,>));
            services.AddScoped(typeof(IClientSideFilterer<,>), typeof(ClientSideFilterer<,>));
            services.AddScoped(typeof(IManyToManyReferenceBLL<,>), typeof(ManyToManyReferenceBLL<,>));

            services.AddTransient(typeof(ColumnMapperSettings<,>));
            services.AddTransient(typeof(IColumnMapperSetting<,>), typeof(EmptyColumnSetting<,>));
            services.AddTransient(typeof(IColumnMapper<,>), typeof(ColumnMapper<,>));
            services.AddTransient(typeof(IOrderedColumnQueryBuilder<,>), typeof(OrderedColumnQueryBuilder<,>));
            
            services.AddScoped(typeof(IUserFieldsToDictionaryResolver), typeof(UserFieldsToDictionaryResolver));

            services.AddMappingScoped(new ServiceMapping<ObjectClass, IProductCatalogTreeQuery>()
                .Map<OwnerProductCatalogTreeQuery>().To(ObjectClass.ProductCatalogue)
                .Map<ProductCatalogCategoryTreeQuery>().To(ObjectClass.ProductCatalogCategory)
                .Map<UnknownProductCatalogTreeQuery>().To(ObjectClass.Unknown));

            services.AddMappingScoped(
                new ServiceMapping<SoftwareModelTemplate, ISoftwareModelProvider>()
                .Map<SoftwareModelProvider<SoftwareCommercialModelDetails>>().To(SoftwareModelTemplate.CommercialModel)
                .Map<SoftwareModelProvider<SoftwarePackageDetails>>().To(SoftwareModelTemplate.SoftwarePackage)
                .Map<SoftwareModelProvider<SoftwareUpgradeDetails>>().To(SoftwareModelTemplate.Upgrade)
                .Map<TechnicalModelSoftwareModelProvider>().To(SoftwareModelTemplate.TechnicalModel)
                .Map<AnotherSoftwareModelProvider<SoftwareComponentDetails>>().To(SoftwareModelTemplate.Component)
                .Map<AnotherSoftwareModelProvider<SoftwareUpdateDetails>>().To(SoftwareModelTemplate.UpdateCorrection)
                );

            services.AddMappingScoped(
                new ServiceMapping<ProductTemplate, IEntityCharacteristicsProvider>()
                .Map<EntityCharacteristicsProvider<CDAndDVDDrives, CDAndDVDDrivesDetails>>().To(ProductTemplate.CdDvdRom)
                .Map<EntityCharacteristicsProvider<Floppydrive, FloppydriveDetails>>().To(ProductTemplate.Fdd)
                .Map<EntityCharacteristicsProvider<Memory, MemoryDetails>>().To(ProductTemplate.Ram)
                .Map<EntityCharacteristicsProvider<Modem, ModemDetails>>().To(ProductTemplate.ModemAdapter)
                .Map<EntityCharacteristicsProvider<Motherboard, MotherboardDetails>>().To(ProductTemplate.MotherBoard)
                .Map<EntityCharacteristicsProvider<NetworkAdapter, NetworkAdapterDetails>>().To(ProductTemplate.NetworkAdapter)
                .Map<EntityCharacteristicsProvider<Processor, ProcessorDetails>>().To(ProductTemplate.Processor)
                .Map<EntityCharacteristicsProvider<Soundcard, SoundcardDetails>>().To(ProductTemplate.SoundCard)
                .Map<EntityCharacteristicsProvider<Storage, StorageDetails>>().To(ProductTemplate.HardDrive)
                .Map<EntityCharacteristicsProvider<StorageController, StorageControllerDetails>>().To(ProductTemplate.DataStorageController)
                .Map<EntityCharacteristicsProvider<VideoAdapter, VideoAdapterDetails>>().To(ProductTemplate.VideoAdapter)
                );

            services.RegisterModels(
                (typeof(AdapterType), ObjectClass.AdapterModel), 
                (typeof(NetworkDeviceModel), ObjectClass.NetworkDeviceModel), 
                (typeof(PeripheralType), ObjectClass.PeripherialModel),
                (typeof(TerminalDeviceModel), ObjectClass.TerminalDeviceModel),
                (typeof(MaterialModel), ObjectClass.MaterialModel),
                (typeof(SoftwareLicenseModel), ObjectClass.SoftwareLicenseModel),
                (typeof(ServiceContractModel), ObjectClass.ServiceContract),
                (typeof(CabinetType), ObjectClass.CabinetType));

            services.RegisterSlotServices();

            services.AddNotificationTemplate();
            services.AddScoped(typeof(IUserFiltrationQueryBuilder<,>), typeof(UserFiltrationQueryBuilder<,>));
            services.AddScoped(typeof(ICreateWorkflow<>), typeof(WorkflowEntityBLL<>));
            services.AddScoped(typeof(ISendWorkflowEntityEvent<>), typeof(WorkflowEntityEventSender<>));

            services.AddScoped(typeof(ICloner<>), typeof(Cloner<>));
            services.AddSingleton<UserPasswordService>();

            services
                .AddScoped<IEquipmentBaseBLL<int, NetworkDevice, NetworkDeviceData, NetworkDeviceDetails>
                , EquipmentBaseBLL<int, NetworkDevice, NetworkDeviceData, NetworkDeviceDetails>>()
                .AddScoped<IEquipmentBaseBLL<Guid, Adapter, AdapterData, AdapterDetails>
                , EquipmentBaseBLL<Guid, Adapter, AdapterData, AdapterDetails>>()
                .AddScoped<IEquipmentBaseBLL<Guid, Peripheral, PeripheralData, PeripheralDetails>
                , EquipmentBaseBLL<Guid, Peripheral, PeripheralData, PeripheralDetails>>();


            services.AddMappingScoped(
                new ServiceMapping<ObjectClass, ISearchQueryCreator>()
                    .Map<SearchQueryCreator<Call>>().To(ObjectClass.Call)
                    .Map<SearchQueryCreator<WorkOrder>>().To(ObjectClass.WorkOrder)
                    .Map<SearchQueryCreator<MassIncident>>().To(ObjectClass.MassIncident)
                    .Map<SearchQueryCreator<Problem>>().To(ObjectClass.Problem)
            );

            services.AddMappingScoped(new ServiceMapping<LifeCycleOperationCommandType, LifeCycleCommandWithAlert>()
                .Map<PutOnControlCommand>().To(LifeCycleOperationCommandType.Registration));

            services.AddMappingScoped(new ServiceMapping<LifeCycleOperationCommandType, LifeCycleCommand>()
                .Map<ChangeLifeCycleStateCommand>().To(LifeCycleOperationCommandType.ChangeLifeCycleState)
                .Map<AddFromStorageCommand>().To(LifeCycleOperationCommandType.AddFromStorage)
                .Map<ToStorageCommand>().To(LifeCycleOperationCommandType.ToStorage)
                .Map<MoveCommand>().To(LifeCycleOperationCommandType.Move));

            services.AddMappingScoped(new ServiceMapping<LifeCycleOperationCommandType, ILifeCycleCommandExecutor>()
                .Map<MoveCommandExecutor>().To(LifeCycleOperationCommandType.Move)
                .Map<ToStorageCommandExecutor>().To(LifeCycleOperationCommandType.ToStorage)
                .Map<AddFromStorageCommandExecutor>().To(LifeCycleOperationCommandType.AddFromStorage));

            services.AddMappingScoped(new ServiceMapping<LifeCycleOperationCommandType, AssetHistorySaveStrategy>()
                .Map<RegistrationHistorySaveStrategy>().To(LifeCycleOperationCommandType.Registration)
                .Map<MoveHistorySaveStrategy>().To(LifeCycleOperationCommandType.ToStorage)
                .Map<MoveHistorySaveStrategy>().To(LifeCycleOperationCommandType.AddFromStorage)
                .Map<MoveHistorySaveStrategy>().To(LifeCycleOperationCommandType.Move));

            services.AddMappingScoped(new ServiceMapping<ObjectClass, IDeviceLocationUpdater>()
                .Map<AdapterLocationUpdater>().To(ObjectClass.Adapter)
                .Map<NetworkDeviceLocationUpdater>().To(ObjectClass.ActiveDevice));

            services.AddMappingScoped(new ServiceMapping<LifeCycleCommandAlertType, IAssignValueForComponentsStrategy>()
                .Map<AssignValueForComponentsStrategy>().To(LifeCycleCommandAlertType.None));

            return services;
        }

        public static ServiceMapping<LookupQueries, ILookupQuery> AddBllLookupQueries(this ServiceMapping<LookupQueries, ILookupQuery> mapping)
        {
            return mapping
                .Map<TaskCategoryLookupQuery>().To(LookupQueries.TaskCategory)
                .Map<IssueCategoryLookupQuery>().To(LookupQueries.IssueCategory)
                .Map<NegotiationModeLookupQuery>().To(LookupQueries.NegotiationMode)
                .Map<NegotiationStatusLookupQuery>().To(LookupQueries.NegotiationStatus)
                .Map<MassIncidentsInformationChannelLookupQuery>().To(LookupQueries.MassIncidentInformationChannels)
                .Map<IsWorkingLookupQuery>().To(LookupQueries.IsWorkingLookup)
                .Map<LocationOnStoreLookupQuery>().To(LookupQueries.LocationOnStoreLookup);
        }
    }
}
