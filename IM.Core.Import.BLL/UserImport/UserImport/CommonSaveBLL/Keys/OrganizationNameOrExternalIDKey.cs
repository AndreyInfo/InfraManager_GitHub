using Mono.Unix.Native;

namespace IM.Core.Import.BLL.Import.Array;

internal sealed class OrganizationNameOrExternalIDKey:OrKeys<OrganizationNameKey,ExternalIDKey>
{
    public OrganizationNameOrExternalIDKey(string name, string externlID) : base(new OrganizationNameKey(name), new ExternalIDKey(externlID))
    {
    }

    public static bool IsSet(string name, string externalID)
    {
        return NamedKey.IsSet(name) || ExternalIDKey.IsSet(externalID);
    }
    
    public override string ToString()
    {
        return $"[{Key}/{OrKey}]";
    }
}