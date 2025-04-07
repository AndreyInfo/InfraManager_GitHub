namespace IM.Core.Import.BLL.Interface.Ldap;

public interface ILdapRequestData
{
    string Url { get; }
    ILdapConnectionData? ConnectionData { get;}
    IBindData BindData { get;}
    ISearchData SearchData { get;}
    
}