using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ProductCatalogue.Models
{
    [ListViewItem(ListView.GuidsPortList)]
    public class PortsListItem
    {
        public Guid ID => default;


        [ColumnSettings(0)]
        [Label(nameof(Resources.Number))]
        public int Number => default;


        [ColumnSettings(1)]
        [Label(nameof(Resources.PortConnector))]
        public string Connector => default;

        [ColumnSettings(2)]
        [Label(nameof(Resources.PortTechnology))]
        public string Tecnology => default;
    }
}