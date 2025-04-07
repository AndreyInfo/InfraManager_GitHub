using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.OrganizationStructure
{
    [ListViewItem(ListView.MyDeputyForTable)]
    public class DeputyUserListItem : BaseDeputyUserListItem
    {
        [LikeFilter]
        [ColumnSettings(0, 250)]
        [Label(nameof(Resources.DeputyProfileSettings))]
        public override string UserFullName { get; init; }
    }
}