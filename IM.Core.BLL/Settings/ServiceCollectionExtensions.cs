using InfraManager.BLL.Settings.TableFilters;
using InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL.Settings.UserFields;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.BLL.Settings
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSettings(this IServiceCollection services)
        {
            services.AddMappingScoped(
                new ServiceMapping<UserFieldType, IUserFieldNameBLL>()
                    .Map<UserFieldNameBLL<AssetUserFieldName>>().To(UserFieldType.Asset)
                    .Map<UserFieldNameBLL<CallUserFieldName>>().To(UserFieldType.Call)
                    .Map<UserFieldNameBLL<ProblemUserFieldName>>().To(UserFieldType.Problem)
                    .Map<UserFieldNameBLL<WorkOrderUserFieldName>>().To(UserFieldType.WorkOrder));

            services.AddMappingScoped(
                new ServiceMapping<string, IBuildTreeSettings>()
                    .Map<UserTreeSettingsBuilder>().To(ObjectSearchers.UserSearcher)
                    .Map<UserTreeSettingsBuilder>().To(ObjectSearchers.WebUserSearcher)
                    .Map<UtilizerTreeSettingsBuilder>().To(ObjectSearchers.UtilizerSearcher)
                    .Map<SubDivisionTreeSettingsBuilder>().To(ObjectSearchers.SubDivisionSearcher)
                    .Map<OwnerTreeSettingsBuilder>().To(ObjectSearchers.OrganizationSearcher)
                    .Map<OwnerTreeSettingsBuilder>().To(ObjectSearchers.OwnerSearcher)
                    .Map<OwnerUserTreeSettingsBuilder>().To(ObjectSearchers.OwnerUserSearcher)
                    .Map<ExecutorUserTreeSettingsBuilder>().To(ObjectSearchers.ExecutorUserSearcher)
                    .Map<AccomplisherUserTreeSettingsBuilder>().To(ObjectSearchers.AccomplisherUserSearcher)
                    .Map<AccomplisherUserTreeSettingsBuilder>().To(ObjectSearchers.MaterialResponsibleUserSearcher)
                    .Map<GroupUserTreeSettingsBuilder>().To(ObjectSearchers.SDExecutorSearcher)
                    .Map<GroupUserTreeSettingsBuilder>().To(ObjectSearchers.UserWithQueueSearcher)
                    .Map<GroupUserTreeSettingsBuilder>().To(ObjectSearchers.UserWithQueueSearcherNoTOZ)
                    .Map<StrictNoTozUserTreeSettingsBuilder>().To(ObjectSearchers.WebUserSearcherStrictNoTOZ)
                    .Map<SubDivisionNoTozTreeSettingsBuilder>().To(ObjectSearchers.SubDivisionSearcherNoTOZ)
                    .Map<OrganizationNoTozTreeSettingsBuilder>().To(ObjectSearchers.OrganizationSearcherNoTOZ)
                    .Map<LocationTreeSettingsBuilder>().To(ObjectSearchers.ParameterLocationSearcher)
                    .Map<BuildingTreeSettingsBuilder>().To(ObjectSearchers.BuildingLocationSearcher)
                    .Map<FloorTreeSettingsBuilder>().To(ObjectSearchers.FloorLocationSearcher)
                    .Map<RoomTreeSettingsBuilder>().To(ObjectSearchers.RoomSearcher)
                    .Map<RackTreeSettingsBuilder>().To(ObjectSearchers.RackSearcher)
                    .Map<WorkplaceTreeSettingsBuilder>().To(ObjectSearchers.WorkplaceSearcher)
                    .Map<NoneTreeSettingsBuilder>().To(ObjectSearchers.PositionSearcher)
                    .Map<NoneTreeSettingsBuilder>().To(ObjectSearchers.ParameterConfigurationItemSearcher)
                    .Map<NoneTreeSettingsBuilder>().To(ObjectSearchers.ParameterModelSearcher)
                    .Map<NoneTreeSettingsBuilder>().To(ObjectSearchers.ProjectAndFinanceActionSearcher)
                    .Map<NoneTreeSettingsBuilder>().To(ObjectSearchers.FinanceCenterSearcher)
                    .Map<NoneTreeSettingsBuilder>().To(ObjectSearchers.ProductCatalogTypeAndModelSearcher)
                    .Map<NoneTreeSettingsBuilder>().To(ObjectSearchers.QueueSearcher)
                    .Map<NoneTreeSettingsBuilder>().To(ObjectSearchers.BudgetSearcher));

            services.AddMappingScoped(
                new ServiceMapping<SystemSettings, IConvertSettingValue>().MapSystemSettingConverters());

            return services;
        }
    }
}
