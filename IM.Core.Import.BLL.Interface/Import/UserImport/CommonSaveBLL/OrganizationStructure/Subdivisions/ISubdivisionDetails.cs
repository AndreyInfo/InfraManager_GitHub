namespace IM.Core.Import.BLL.Interface.Import;


/// <summary>
/// Интерфейс для SubdivisionDetails
/// </summary>
public interface ISubdivisionDetails
{
    Guid? OrganizationID { get; set; }
    string OrganisationName { get; set; }
    string OrganisationExternalId { get; set; }
    OrganizationNameThenExternalIdKey OrganizationOrganizationNameThenExternalIdKey { get; set; }
    SubdivisionNameThenExternalIdKey SubdivisionOrganizationNameThenExternalIdKey { get; set; }
    public string Name { get; set; }
    Guid? SubdivisionID { get; set; }
    string Note { get; set; }
    string ExternalID { get; set; }
    Guid? PeripheralDatabaseID { get; set; }
    Guid? ComplementaryID { get; set; }
    Guid? CalendarWorkScheduleID { get; set; }
    bool? IsLockedForOsi { get; set; }
    bool It { get; set; }
    bool IsValid();

    bool IsNoSubdivision();
    
    IEnumerable<string> ParentFullName { get; }
    
    string ParentExternalID { get; set; }
    
    ISubdivisionDetails ParentSubdivision { get; set; }
}