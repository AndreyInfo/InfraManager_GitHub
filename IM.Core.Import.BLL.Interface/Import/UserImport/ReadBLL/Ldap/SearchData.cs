namespace IM.Core.Import.BLL.Interface.Ldap;

public class SearchData : ISearchData
{
    private const int DefaultPageSize = 900;

    public SearchData(string mask, int scope, string baseNode, int? pageSize = null)
    {
        Mask = mask;
        Scope = scope;
        BaseNode = baseNode;
        PageSize = pageSize ?? DefaultPageSize;
    }

    public string Mask { get; init; }
    
    public int Scope { get; init; }

    public int PageSize { get; }

    public string BaseNode { get; init; }

}