using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.AssetsManagement.Hardware;
using InfraManager.BLL.ProductCatalogue;
using InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models;
using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.Asset;
using InfraManager.DAL.AssetsManagement.Hardware;
using InfraManager.DAL.ConfigurationData;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.BLL.AssetsManagement;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddAssetsManagement(this IServiceCollection services)
    {
        services
            .AddScoped<IBuildEntityQuery<AdapterType, ProductModelDetails, ProductModelListFilter>,
                ProductModelQueryBuilder<AdapterType, ProductModelDetails, ProductModelListFilter>>()
            .AddScoped<IBuildEntityQuery<PeripheralType, ProductModelDetails, ProductModelListFilter>,
                ProductModelQueryBuilder<PeripheralType, ProductModelDetails, ProductModelListFilter>>()
            .AddScoped<IBuildEntityQuery<NetworkDeviceModel, ProductModelDetails, ProductModelListFilter>,
                ProductModelQueryBuilder<NetworkDeviceModel, ProductModelDetails, ProductModelListFilter>>()
            .AddScoped<IBuildEntityQuery<TerminalDeviceModel, ProductModelDetails, ProductModelListFilter>,
                ProductModelQueryBuilder<TerminalDeviceModel, ProductModelDetails, ProductModelListFilter>>();

        services
            .AddScoped<IStandardPredicatesProvider<Adapter>, StandardPredicatesDictionary<Adapter>>()
            .AddScoped<IStandardPredicatesProvider<Peripheral>, StandardPredicatesDictionary<Peripheral>>()
            .AddScoped<IStandardPredicatesProvider<TerminalDevice>, StandardPredicatesDictionary<TerminalDevice>>()
            .AddScoped<IStandardPredicatesProvider<NetworkDevice>, StandardPredicatesDictionary<NetworkDevice>>()
            .AddScoped<IStandardPredicatesProvider<DataEntity>, StandardPredicatesDictionary<DataEntity>>();

        // Список оборудования Hardware
        services
            .AddScoped<
                IAggregatePredicateBuilders<Adapter, HardwareListItem>,
                HardwareListAdapterPredicateBuilders<HardwareListItem>>()
            .AddScoped<
                IAggregatePredicateBuilders<Peripheral, HardwareListItem>,
                HardwareListPeripheralPredicateBuilders<HardwareListItem>>()
            .AddScoped<
                IAggregatePredicateBuilders<TerminalDevice, HardwareListItem>,
                HardwareListTerminalDevicePredicateBuilders<HardwareListItem>>()
            .AddScoped<
                IAggregatePredicateBuilders<NetworkDevice, HardwareListItem>,
                HardwareListNetworkDevicePredicateBuilders<HardwareListItem>>()

            .AddScoped<
                IAggregatePredicateBuilders<AllHardwareListQueryResultItem, HardwareListItem>,
                HardwareListItemPredicateBuilders<AllHardwareListQueryResultItem, HardwareListItem>>()

            .AddListView<HardwareListItem, AllHardwareListFilter>()
            .AsUnion<AllHardwareListQueryResultItem>(
                x => x.Query<Adapter>(),
                x => x.Query<Peripheral>(),
                x => x.Query<TerminalDevice>(),
                x => x.Query<NetworkDevice>());

        // Список оборудования AssetSearch
        services
            .AddScoped<
                IAggregatePredicateBuilders<Adapter, AssetSearchListItem>,
                HardwareListAdapterPredicateBuilders<AssetSearchListItem>>()
            .AddScoped<
                IAggregatePredicateBuilders<Peripheral, AssetSearchListItem>,
                HardwareListPeripheralPredicateBuilders<AssetSearchListItem>>()
            .AddScoped<
                IAggregatePredicateBuilders<TerminalDevice, AssetSearchListItem>,
                HardwareListTerminalDevicePredicateBuilders<AssetSearchListItem>>()
            .AddScoped<
                IAggregatePredicateBuilders<NetworkDevice, AssetSearchListItem>,
                HardwareListNetworkDevicePredicateBuilders<AssetSearchListItem>>()
            .AddScoped<
                IAggregatePredicateBuilders<DataEntity, AssetSearchListItem>,
                FilterBuildersAggregate<DataEntity, AssetSearchListItem>>()

            .AddScoped<
                IAggregatePredicateBuilders<AssetSearchListQueryResultItem, AssetSearchListItem>,
                HardwareListItemPredicateBuilders<AssetSearchListQueryResultItem, AssetSearchListItem>>()

            .AddTransient<IBuildListViewFilterPredicates<DataEntity, AssetSearchListFilter>, AssetSearchListDataEntityFilterBuilder>()
            .AddTransient<IBuildListViewFilterPredicates<Adapter, AssetSearchListFilter>, AssetSearchListAdapterFilterBuilder>()
            .AddTransient<IBuildListViewFilterPredicates<Peripheral, AssetSearchListFilter>, AssetSearchListPeripheralFilterBuilder>()
            .AddTransient<IBuildListViewFilterPredicates<TerminalDevice, AssetSearchListFilter>, AssetSearchListTerminalDeviceFilterBuilder>()
            .AddTransient<IBuildListViewFilterPredicates<NetworkDevice, AssetSearchListFilter>, AssetSearchListNetworkDeviceFilterBuilder>()

            .AddListView<AssetSearchListItem, AssetSearchListFilter>()
            .AsUnion<AssetSearchListQueryResultItem>(
                x => x.Query<DataEntity>(),
                x => x.Query<Adapter>(),
                x => x.Query<Peripheral>(),
                x => x.Query<TerminalDevice>(),
                x => x.Query<NetworkDevice>());

        // Список оборудования клиента
        services
            .AddScoped<
                IAggregatePredicateBuilders<Peripheral, ClientsHardwareListItem>,
                FilterBuildersAggregate<Peripheral, ClientsHardwareListItem>>()
            .AddScoped<
                IAggregatePredicateBuilders<TerminalDevice, ClientsHardwareListItem>,
                FilterBuildersAggregate<TerminalDevice, ClientsHardwareListItem>>()

            .AddScoped<
                IAggregatePredicateBuilders<AllHardwareListQueryResultItem, ClientsHardwareListItem>,
                FilterBuildersAggregate<AllHardwareListQueryResultItem, ClientsHardwareListItem>>()
            
            .AddTransient<IBuildListViewFilterPredicates<Peripheral, ClientsHardwareListFilter>, ClientsHardwareListFilterBuilder<Peripheral>>()
            .AddTransient<IBuildListViewFilterPredicates<TerminalDevice, ClientsHardwareListFilter>, ClientsHardwareListFilterBuilder<TerminalDevice>>()

            .AddListView<ClientsHardwareListItem, ClientsHardwareListFilter>()
            .AsUnion<AllHardwareListQueryResultItem>(
                x => x.Query<Peripheral>(),
                x => x.Query<TerminalDevice>());

        return services;
    }
}