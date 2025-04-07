using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL.ListView;

namespace InfraManager.BLL.AssetsManagement.Hardware;

internal class HardwareBLL : IHardwareBLL, ISelfRegisteredService<IHardwareBLL>
{
    private readonly IListViewBLL<HardwareListItem, AllHardwareListFilter> _allHardwareListBuilder;
    private readonly IListViewBLL<AssetSearchListItem, AssetSearchListFilter> _assetSearchListBuilder;
    private readonly IListViewBLL<ClientsHardwareListItem, ClientsHardwareListFilter> _clientsHardwareListBuilder;

    public HardwareBLL(
        IListViewBLL<HardwareListItem, AllHardwareListFilter> allHardwareListBuilder,
        IListViewBLL<AssetSearchListItem, AssetSearchListFilter> assetSearchListBuilder,
        IListViewBLL<ClientsHardwareListItem, ClientsHardwareListFilter> clientsHardwareListBuilder
        )
    {
        _allHardwareListBuilder = allHardwareListBuilder;
        _assetSearchListBuilder = assetSearchListBuilder;
        _clientsHardwareListBuilder = clientsHardwareListBuilder;
    }

    public Task<HardwareListItem[]> AllHardwareAsync(
        ListViewFilterData<AllHardwareListFilter> filterBy,
        CancellationToken cancellationToken = default)
    {
        return _allHardwareListBuilder.BuildAsync(filterBy, cancellationToken);
    }

    public Task<AssetSearchListItem[]> GetReportAsync(
        ListViewFilterData<AssetSearchListFilter> filterBy,
        CancellationToken cancellationToken = default)
    {
        return _assetSearchListBuilder.BuildAsync(filterBy, cancellationToken);
    }

    public Task<ClientsHardwareListItem[]> GetClientsHardwareListAsync(
        ListViewFilterData<ClientsHardwareListFilter> filterBy,
        CancellationToken cancellationToken = default)
    {
        return _clientsHardwareListBuilder.BuildAsync(filterBy, cancellationToken);
    }
}