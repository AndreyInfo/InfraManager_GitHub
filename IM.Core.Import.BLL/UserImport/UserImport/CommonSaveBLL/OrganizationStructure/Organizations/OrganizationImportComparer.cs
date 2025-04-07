using IM.Core.Import.BLL.Interface.Import;
using InfraManager;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.Import;

internal class OrganizationImportComparer : ImportMapperComparer<OrganizationDetails,Organization>, 
    ISelfRegisteredService<IImportMapperComparer<OrganizationDetails,Organization>>
{
    protected override List<KeyValuePair<ObjectType, Func<OrganizationDetails, Organization, bool>>> Comparers { get; } = new()
    {
        new KeyValuePair<ObjectType, Func<OrganizationDetails, Organization, bool>>(ObjectType.OrganizationExternalID, (x, y) => x.ExternalId == y.ExternalId),
        new KeyValuePair<ObjectType, Func<OrganizationDetails, Organization, bool>>(ObjectType.OrganizationName, (x, y) => x.Name == y.Name),
        new KeyValuePair<ObjectType, Func<OrganizationDetails, Organization, bool>>(ObjectType.OrganizationNote, (x, y) => x.Note == y.Note),
    };

    protected override void SetIgnoreCompareFields(ObjectType flags, List<KeyValuePair<ObjectType, Func<OrganizationDetails, Organization, bool>>> fieldComparers)
    {
        IgnoreComparerIf(flags, ObjectType.OrganizationName, fieldComparers);
        IgnoreComparerIf(flags, ObjectType.OrganizationNote, fieldComparers);
        IgnoreComparerIf(flags, ObjectType.OrganizationExternalID, fieldComparers);
    }
}