using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Sessions;

[ListViewItem(ListView.UserPersonalLicences)]
public class UserPersonalLicenceListItem
{
    public Guid UserID { get; init; }
    
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.AdminTools_PersonalLicence_UserFullName))]
    public string FullName { get { return default; } }
    
    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.AdminTools_PersonalLicence_UserSubdivisionFullName))]
    public string SubdivisionFullName { get { return default; } }
    
    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.AdminTools_PersonalLicence_UserNumber))]
    public string Number { get { return default; } }
    
    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.AdminTools_PersonalLicence_UserLoginName))]
    public string LoginName { get { return default; } }
    
    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.AdminTools_PersonalLicence_DateCreated))]
    public string UtcDateCreated { get { return default; } }
}