using System;
using InfraManager.ResourcesArea;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using Inframanager.BLL.ListView;
using Inframanager.BLL;

namespace InfraManager.BLL.Asset.ForTable
{
    [ListViewItem(ListView.InfrastructureSegment)]
    public class InfrastructureSegmentForTable
    {
        public Guid ID { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(1)]
        [Label(nameof(Resources.Name))]
        public string Name { get; init; }
    }
}
