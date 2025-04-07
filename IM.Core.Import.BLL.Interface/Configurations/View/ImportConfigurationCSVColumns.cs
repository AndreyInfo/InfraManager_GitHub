using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL;
using InfraManager.ResourcesArea;

namespace IM.Core.Import.BLL.Interface.Configurations.View
{
    [ListViewItem("ImportConfigurationCSV")]

    public class ImportConfigurationCSVColumns
    {
        [ColumnSettings(1, 200)]
        [Label(nameof(Resources.Name))]
        public string Name { get; }

        [ColumnSettings(2, 200)]
        [Label(nameof(Resources.LinkNote))]
        public string Note { get; }

    }
}
