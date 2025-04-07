using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceDesk
{
    [ListViewItem(ListView.MyTasksList)]
    public class MyTasksReportItem : ServiceDeskListItem
    {
        [MultiSelectFilter(LookupQueries.TaskCategory)]
        [ColumnSettings(6)]
        [Label(nameof(Resources.LinkCategory))]
        [ColumnSort(nameof(MyTasksListQueryResultItem.CategorySortColumn))]
        public string CategoryName { get; init; }
    }
}
