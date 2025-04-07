
using InfraManager.BLL;
using InfraManager.BLL.AssetsManagement.Hardware;
using Inframanager.BLL.ListView;

namespace InfraManager.UI.Web.Controllers.Api.AssetsManagement;

internal static class ListFilterExtensions
{
    public static ListViewFilterData<AllHardwareListFilter> ToAllHardwareListFilter(this ListFilter filter)
    {
        return new ListViewFilterData<AllHardwareListFilter>
        {
            ExtensionFilter = new AllHardwareListFilter
            {
                IDList = filter.IDList,
            },
            CurrentFilterID = filter.CurrentFilterID,
            CustomFilters = filter.CustomFilters,
            StandardFilter = filter.StandardFilter,
            ViewName = filter.ViewName,
            Take = filter.CountRecords,
            Skip = filter.StartRecordIndex,
        };
    }

    public static ListViewFilterData<AssetSearchListFilter> ToAssetSearchListFilter(this AssetSearchListFilter filter)
    {
        return new ListViewFilterData<AssetSearchListFilter>
        {
            ExtensionFilter = filter,
            CurrentFilterID = filter.CurrentFilterID,
            CustomFilters = filter.CustomFilters,
            StandardFilter = filter.StandardFilter,
            ViewName = filter.ViewName,
            Take = filter.CountRecords,
            Skip = filter.StartRecordIndex,
        };
    }

    public static ListViewFilterData<ClientsHardwareListFilter> ToClientsHardwareListFilter(this ClientsHardwareListFilter filter)
    {
        return new ListViewFilterData<ClientsHardwareListFilter>
        {
            ExtensionFilter = filter,
            CurrentFilterID = filter.CurrentFilterID,
            CustomFilters = filter.CustomFilters,
            StandardFilter = filter.StandardFilter,
            ViewName = filter.ViewName,
            Take = filter.CountRecords,
            Skip = filter.StartRecordIndex,
        };
    }
}