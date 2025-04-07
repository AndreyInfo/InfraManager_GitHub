using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Sessions;

[ListViewItem(ListView.Sessions)]
public class SessionListItem
{
    public Guid UserID { get { return default; } }
    
    public string SecurityStamp { get { return default; } }
    
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.AdminTools_SessionTable_DateOpened))]
    public DateTime UtcDateOpened { get { return default; } }
    
    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.AdminTools_SessionTable_DateClosed))]
    public DateTime UtcDateClosed { get { return default; } }
    
    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.AdminTools_SessionTable_DateLastActivity))]
    public DateTime UtcDateLastActivity { get { return default; } }
    
    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.AdminTools_SessionTable_UserAgent))]
    public string UserAgent { get { return default; } }
    
    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.AdminTools_SessionTable_UserFullName))]
    public string UserName { get { return default; } }
    
    [ColumnSettings(6, 100)]
    [Label(nameof(Resources.AdminTools_SessionTable_UserLogin))]
    public string UserLogin { get { return default; } }
    
    [ColumnSettings(7, 100)]
    [Label(nameof(Resources.AdminTools_SessionTable_UserSubdivisionFullName))]
    public string UserSubdivisionFullName { get { return default; } }
    
    [ColumnSettings(8, 100)]
    [Label(nameof(Resources.AdminTools_SessionTable_DurationInMinutes))]
    public string DurationInMinutes { get { return default; } }
    
    [ColumnSettings(9, 100)]
    [Label(nameof(Resources.AdminTools_SessionTable_ConnectionPoint))]
    public string LocationName { get { return default; } }
    
    [ColumnSettings(10, 100)]
    [Label(nameof(Resources.AdminTools_SessionTable_Licence))]
    public string LicenceType { get { return default; } }
}