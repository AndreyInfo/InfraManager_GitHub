using IM.Core.Import.BLL.Interface.Import;
using InfraManager;
using InfraManager.DAL.Import;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.Import;

internal class SubdivisionMapperComparer : ImportMapperComparer<ISubdivisionDetails, Subdivision>, ISelfRegisteredService<IImportMapperComparer<ISubdivisionDetails, Subdivision>>
{
    protected override List<KeyValuePair<ObjectType, Func<ISubdivisionDetails, Subdivision, bool>>> Comparers { get; } = new()
    {
        new KeyValuePair<ObjectType, Func<ISubdivisionDetails, Subdivision, bool>>(ObjectType.SubdivisionExternalID, (x, y) => x.ExternalID == y.ExternalID),
        new KeyValuePair<ObjectType, Func<ISubdivisionDetails, Subdivision, bool>>(ObjectType.SubdivisionName, (x, y) => x.Name == y.Name),
        new KeyValuePair<ObjectType, Func<ISubdivisionDetails, Subdivision, bool>>(ObjectType.SubdivisionNote, (x, y) => x.Note == y.Note),
        new KeyValuePair<ObjectType, Func<ISubdivisionDetails, Subdivision, bool>>(ObjectType.SubdivisionOrganization, (x, y) => x.OrganizationID == y.OrganizationID),
        new KeyValuePair<ObjectType, Func<ISubdivisionDetails, Subdivision, bool>>(ObjectType.SubdivisionOrganizationExternalID, (x, y) => x.OrganizationID == y.OrganizationID),
        new KeyValuePair<ObjectType, Func<ISubdivisionDetails, Subdivision, bool>>(ObjectType.SubdivisionParentExternalID, (x, y) => x.SubdivisionID == y.SubdivisionID),
        new KeyValuePair<ObjectType, Func<ISubdivisionDetails, Subdivision, bool>>(ObjectType.SubdivisionParent, (x, y) => x.SubdivisionID == y.SubdivisionID)
    };

    protected override void SetIgnoreCompareFields(ObjectType flags, List<KeyValuePair<ObjectType, Func<ISubdivisionDetails, Subdivision, bool>>> fieldComparers)
    {
        IgnoreComparerIf(flags, ObjectType.SubdivisionName, fieldComparers);
        IgnoreComparerIf(flags, ObjectType.SubdivisionNote, fieldComparers);
        IgnoreComparerIf(flags, ObjectType.SubdivisionExternalID, fieldComparers);
        IgnoreComparerIf(flags, ObjectType.SubdivisionOrganization, fieldComparers);
        IgnoreComparerIf(flags, ObjectType.SubdivisionOrganizationExternalID, fieldComparers);
        IgnoreComparerIf(flags, ObjectType.SubdivisionParent, fieldComparers);
        IgnoreComparerIf(flags, ObjectType.SubdivisionParentExternalID, fieldComparers);
    }

}