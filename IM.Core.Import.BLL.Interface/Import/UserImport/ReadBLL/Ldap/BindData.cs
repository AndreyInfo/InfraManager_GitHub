namespace IM.Core.Import.BLL.Interface.Ldap;

public class BindData : IBindData
{
    public BindData(string dn, string password, int version)
    {
        Dn = dn;
        Password = password;
        Version = version;
    }

    public string Dn { get; init; }
    
    public string Password { get; init; }

    public int Version { get; init; }
}
