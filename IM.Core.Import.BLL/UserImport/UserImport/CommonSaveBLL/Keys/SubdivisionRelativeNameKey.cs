namespace IM.Core.Import.BLL.Import.Array;

internal sealed record SubdivisionRelativeNameKey(Guid? organizationID,
    Guid? parentSubdivisionID,
    string subdivisionName) :
    RequiredComplexKey<ComplexKey<OrganizationIDKey, ParentSubdivisionIDKey>, SubdivisionSimpleNameKey>(
        new ComplexKey<OrganizationIDKey, ParentSubdivisionIDKey>(new OrganizationIDKey(organizationID),
            new ParentSubdivisionIDKey(parentSubdivisionID)),
        new SubdivisionSimpleNameKey(subdivisionName))
{
    public override string ToString()
    {
        return $"[{First.Required}, {LogHelper.ToOutputFormat(First.Optional?.ToString())}, {Second}]";
    }
}