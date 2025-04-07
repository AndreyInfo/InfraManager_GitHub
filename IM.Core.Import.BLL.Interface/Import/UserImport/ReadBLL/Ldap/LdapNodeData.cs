namespace IM.Core.Import.BLL.Interface.Ldap;

public record LdapNodeData(string Dn,
   
    IEnumerable<IReadOnlyDictionary<string, string>> Parents,
    IReadOnlyDictionary<string, string> Attributes) : ILdapDataEntry
{
    public LdapNodeData? Manager { get; set; }
    
    public string Address { get; set; }
    
}