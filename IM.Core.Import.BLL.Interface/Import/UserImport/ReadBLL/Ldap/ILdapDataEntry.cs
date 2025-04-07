namespace IM.Core.Import.BLL.Interface.Ldap;

public interface ILdapDataEntry
{
    string Dn { get; }
    
    IReadOnlyDictionary<string, string>? Attributes { get; }
    
    IEnumerable<IReadOnlyDictionary<string, string>> Parents { get; }
    string Address { get; set; }
}