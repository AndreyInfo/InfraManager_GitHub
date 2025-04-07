using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

[ListViewItem(ListView.WorkOrderTemplate)]
public sealed class WorkOrderTemplateListItem
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get { return default; } }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.Folder))]
    public string FolderName { get { return default; } }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.Note))]
    public string Description { get { return default; } }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.InitiatorReally))]
    public string InitiatorName { get { return default; } }

    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.CallPriority))]
    public string WorkOrderPriorityName { get { return default; } }

    [ColumnSettings(6, 100)]
    [Label(nameof(Resources.AutoAsssignment))]
    public string ExecutorAssignmentType { get { return default; } }

    [ColumnSettings(7, 100)]
    [Label(nameof(Resources.Normative))]
    public string ManhoursNormInMinutes { get { return default; } }
}

