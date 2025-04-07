using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.OrganizationStructure.JobTitles;

[ListViewItem(ListView.JobTitleColumns)]
public sealed class JobTitleColumns
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; }
}
