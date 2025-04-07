using Inframanager.BLL;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceDesk
{
    public class PlugViewListItem : ServiceDeskListItemBase
    {
        [ColumnSettings(99, false, true, false)]
        [Label(nameof(Resources.PlugView))]
        public bool PlugView => default;
    }
}
