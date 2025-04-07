using IM.Core.Import.BLL.Interface.Import;

namespace IM.Core.Import.BLL.Import.Array;

public class SubdivisionDetailsWithParentNames:ISubdivisionDetails, ISubdivisionDetailsWithParentNames
{
    private readonly ISubdivisionDetails _details;

    public SubdivisionDetailsWithParentNames(ISubdivisionDetails details)
    {
        _details = details;
    }

    public Guid? OrganizationID
    {
        get => _details.OrganizationID;
        set => _details.OrganizationID= value;
    }

    public string OrganisationName
    {
        get => _details.OrganisationName;
        set => _details.OrganisationName = value;
    }

    public string OrganisationExternalId
    {
        get => _details.OrganisationExternalId;
        set => _details.OrganisationExternalId = value;
    }

    public OrganizationNameThenExternalIdKey OrganizationOrganizationNameThenExternalIdKey
    {
        get => _details.OrganizationOrganizationNameThenExternalIdKey;
        set => _details.OrganizationOrganizationNameThenExternalIdKey = value;
    }

    public SubdivisionNameThenExternalIdKey SubdivisionOrganizationNameThenExternalIdKey
    {
        get => _details.SubdivisionOrganizationNameThenExternalIdKey;
        set => _details.SubdivisionOrganizationNameThenExternalIdKey = value;
    }

    public string Name
    {
        get => _details.Name;
        set => _details.Name = value;
    }

    public Guid? SubdivisionID
    {
        get => _details.SubdivisionID;
        set => _details.SubdivisionID = value;
    }

    public string Note
    {
        get => _details.Name;
        set => _details.Name = value;
    }

    public string ExternalID
    {
        get => _details.ExternalID;
        set => _details.ExternalID = value;
    }

    public Guid? PeripheralDatabaseID
    {
        get => _details.PeripheralDatabaseID;
        set => _details.PeripheralDatabaseID = value;
    }

    public Guid? ComplementaryID
    {
        get => _details.ComplementaryID;
        set => _details.ComplementaryID = value;
    }

    public Guid? CalendarWorkScheduleID
    {
        get => _details.CalendarWorkScheduleID;
        set => _details.CalendarWorkScheduleID = value;
    }

    public bool? IsLockedForOsi
    {
        get => _details.IsLockedForOsi;
        set => _details.IsLockedForOsi = value;
    }

    public bool It
    {
        get => _details.It;
        set => _details.It = value;
    }

    public bool IsValid() => _details.IsValid();


    public bool IsNoSubdivision() => _details.IsNoSubdivision();

    public IEnumerable<string> ParentFullName => _details.ParentFullName;

    public string ParentExternalID
    {
        get => _details.ParentExternalID;
        set => _details.ParentExternalID = value;
    }

    public ISubdivisionDetails ParentSubdivision { get; set; }

    public IEnumerable<string> SubdivisionParent { get; set; }
}