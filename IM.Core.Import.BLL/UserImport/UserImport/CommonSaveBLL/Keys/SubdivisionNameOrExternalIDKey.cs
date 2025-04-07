namespace IM.Core.Import.BLL.Import.Array;

internal sealed class SubdivisionNameOrExternalIDKey:OrKeys<SubdivisionRelativeNameKey, ExternalIDKey>
{
    public SubdivisionNameOrExternalIDKey(Guid? organizationID,
        Guid? subdivionID,
        string name,
        string externalID) : base(new SubdivisionRelativeNameKey(organizationID,
            subdivionID,
            name),
        new ExternalIDKey(externalID))
    {
    }

    public override string ToString()
    {
        return $"[{Key}/{OrKey}]";
    }
}