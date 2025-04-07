using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.DAL.Sessions;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Sessions;

[ListViewItem(ListView.UserSessionHistory)]
public class UserSessionHistoryListItem
{
    public Guid ID { get { return default; } }
    
    public SessionHistoryType Type { get { return default; } }
    
    public Guid UserID { get { return default; } }
    
    public Guid? ExecutorID { get { return default; } }
    
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.AdminTools_SessionHistoryTable_Date))]
    public DateTime UtcDate { get { return default; } }
    
    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.AdminTools_SessionHistoryTable_UserAgent))]
    public string UserAgent { get { return default; } }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.AdminTools_SessionHistoryTable_UserFullName))]
    public string UserFullName { get { return default; } }
    
    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.AdminTools_SessionHistoryTable_EventType))]
    public string TypeString { get { return default; } }
    
    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.AdminTools_SessionHistoryTable_ExecutorFullName))]
    public string ExecutorFullName { get { return default; } }
}