using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.Asset.ForTable
{
    [ListViewItem(ListView.FileSystem)]
    public class FileSystemForTable
    {
        public Guid ID { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(1)]
        [Label(nameof(Resources.Name))]
        public string Name { get; init; }
    }
}
