namespace IM.Core.Import.BLL.Interface.Ldap;

public interface IBindData
{
    string Dn { get;}
    string Password { get;}
    int Version { get;}
}