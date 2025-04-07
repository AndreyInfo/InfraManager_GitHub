using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.OrganizationStructure
{
    [ListViewItem(ListView.IDeputyForTable)]
    public class IDeputyForUserListItem : BaseDeputyUserListItem
    {
        [LikeFilter]
        [ColumnSettings(0, 250)]
        [Label(nameof(Resources.SubstituteProfileSettings))]
        public override string UserFullName { get; init; }
    }
}
