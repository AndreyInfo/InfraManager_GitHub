using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceDesk.CustomControl
{
    [ListViewItem(ListView.CustomControlList)]
    public class ObjectUnderControl : ServiceDeskListItem
    {
        public ObjectUnderControl()
        {
            InControl = true;
        }

        [MultiSelectFilter(LookupQueries.IssueCategory)]
        [ColumnSettings(6)]
        [Label(nameof(Resources.LinkCategory))]
        [ColumnSort(nameof(MyTasksListQueryResultItem.CategorySortColumn))]
        public string CategoryName { get; init; }
    }
}
