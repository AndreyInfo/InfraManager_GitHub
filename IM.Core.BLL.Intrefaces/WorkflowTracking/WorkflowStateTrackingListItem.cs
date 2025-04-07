using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.WorkflowTracking;

[ListViewItem(ListView.WorkflowStateTrackingList)]
public class WorkflowStateTrackingListItem
{
    public Guid? ExecutorID { get; }
    public int TimeSpanInWorkMinutes { get; }
    public int StateId { get; }
    
    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.Name))]
    public string StateName { get; }
    
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.TimeStart))]
    public DateTime UtcEnteredAt { get; }
    
    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.Executor))]
    public string ExecutorName { get; }
    
    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.End))]
    public DateTime UtcLeavedAt { get; }
}