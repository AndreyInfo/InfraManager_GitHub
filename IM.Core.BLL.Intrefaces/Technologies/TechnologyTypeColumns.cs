using System;
using InfraManager.ResourcesArea;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using Inframanager.BLL.ListView;
using Inframanager.BLL;

namespace InfraManager.BLL.Configuration.Technologies;

[ListViewItem(ListView.TechnologyType)]
public sealed class TechnologyTypeColumns
{
    [RangeSliderFilter(false)]
    [ColumnSettings(1)]
    [Label(nameof(Resources.Name))]
    public string Name { get; init; }
}
