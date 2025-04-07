using IM.Core.Import.BLL.Interface.Ldap;

namespace IM.Core.Import.BLL.Ldap;

public class IMLdapTreeContext
{
    private readonly IMLdapContext _context;
    private LdapNodeDescription? _baseNode;
    private readonly LdapUrlData _url;
    private readonly HashSet<string> _returned = new();
    private readonly IMLdapForestContext _forest;

    public IMLdapTreeContext(string? current, IMLdapForestContext forest)
    {
        _context = new IMLdapContext(current);
        _url = new LdapUrlData(current);
        _forest = forest;
    }

    public IMLdapContext Tree => _context;

    public bool IsReturnedOrPlanned(string url)
    {
        return _returned.Contains(url) || _context.HasPlanned(url);
    }

    public bool HasParent(string current)
    {
        var invariantAddress = new LdapUrlData(current).Address;
        if (_baseNode == null)
            return false;
        return _baseNode.HasParent(invariantAddress);
    }

    // public void AddPlanned(string current)
    // {
    //     LdapUrlData ldapNodeData = new LdapUrlData(current);
    //     _context.AddPlanned(ldapNodeData);
    // }

    public void SetBase(LdapNodeDescription baseNode)
    {
        _baseNode = baseNode;
    }

    public void Readed(string dn)
    {
        var address = _url.GetUrlForDn(dn);
        _returned.Add(address);
    }

    public void AddReferences(string[] referenceUrls)
    {
       _forest.AddAddress(referenceUrls);
    }
}