using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue;

[ListViewItem(ListView.SLAInsertColumns)]
internal class SLAInsertColumns
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.ConcludedWith))]
    public string ConcludedWith { get; }
}
