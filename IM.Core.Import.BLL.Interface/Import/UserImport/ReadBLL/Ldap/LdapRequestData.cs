namespace IM.Core.Import.BLL.Interface.Ldap;

public class LdapRequestData : ILdapRequestData
{
    public LdapRequestData(ILdapConnectionData connectionData, IBindData bindData, ISearchData searchData, string url)
    {
        ConnectionData = connectionData;
        BindData = bindData;
        SearchData = searchData;
        Url = url;
    }

    public string Url { get; }

   public ILdapConnectionData ConnectionData { get; init; }
    
    public IBindData BindData { get; init; }
    
    public ISearchData SearchData { get; init; }
}