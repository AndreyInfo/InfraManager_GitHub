using InfraManager.DAL.Asset;
using InfraManager.DAL.AssetsManagement.Hardware;
using InfraManager.DAL.ConfigurationData;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.DAL.AssetsManagement;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHardwareQuery(this IServiceCollection services)
    {
        services
            .AddScoped<
                IListQuery<Adapter, AllHardwareListQueryResultItem>,
                HardwareListAdaptersSubQuery<AllHardwareListQueryResultItem>>()
            .AddScoped<
                IListQuery<Peripheral, AllHardwareListQueryResultItem>,
                HardwareListPeripheralsSubQuery<AllHardwareListQueryResultItem>>()
            .AddScoped<
                IListQuery<TerminalDevice, AllHardwareListQueryResultItem>,
                HardwareListTerminalDevicesSubQuery<AllHardwareListQueryResultItem>>()
            .AddScoped<
                IListQuery<NetworkDevice, AllHardwareListQueryResultItem>,
                HardwareListNetworkDevicesSubQuery<AllHardwareListQueryResultItem>>();

        services
            .AddScoped<
                IListQuery<Adapter, AssetSearchListQueryResultItem>,
                HardwareListAdaptersSubQuery<AssetSearchListQueryResultItem>>()
            .AddScoped<
                IListQuery<Peripheral, AssetSearchListQueryResultItem>,
                HardwareListPeripheralsSubQuery<AssetSearchListQueryResultItem>>()
            .AddScoped<
                IListQuery<TerminalDevice, AssetSearchListQueryResultItem>,
                HardwareListTerminalDevicesSubQuery<AssetSearchListQueryResultItem>>()
            .AddScoped<
                IListQuery<NetworkDevice, AssetSearchListQueryResultItem>,
                HardwareListNetworkDevicesSubQuery<AssetSearchListQueryResultItem>>()
            .AddScoped<
                IListQuery<DataEntity, AssetSearchListQueryResultItem>,
                HardwareListDataEntitiesSubQuery>();

        return services;
    }
}