namespace IM.Core.Import.BLL.Interface.Ldap;

public interface ISearchData
{
    string Mask { get;}
    int Scope { get;}
    
    int PageSize { get; }
    
    string BaseNode { get; }
}