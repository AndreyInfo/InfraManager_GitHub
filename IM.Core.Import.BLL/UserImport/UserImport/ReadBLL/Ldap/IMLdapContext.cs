using System.Collections;
using IM.Core.Import.BLL.Interface.Ldap;

namespace IM.Core.Import.BLL.Ldap;

public class IMLdapContext
{
    private readonly HashSet<string> passedDns = new();
    private readonly HashSet<LdapNodeDescription> plannedNodes = new();
    private readonly LdapUrlData _currentTree;
    private LdapNodeDescription? _current;

    public bool HasPlanned(string address)
    {
        var ldapUrlData = new LdapUrlData(address);
        var invariantAddress = ldapUrlData.Address;
        return plannedNodes.Any(x => x.Url.Address == invariantAddress);
    }

    public IMLdapContext(string address)
    {
        _currentTree = new LdapUrlData(address);
    }

    public void AddPlanned(LdapNodeDescription topRequest)
    {
        plannedNodes.Add(topRequest);
    }

    public bool AnyPlanned()
    {
        return plannedNodes.Any();
    }

    public LdapNodeDescription GetNext()
    {
        return plannedNodes.First();
    }

    private void Passed(LdapNodeDescription current)
    {
        plannedNodes.Remove(current);
        passedDns.Add(current.Url.Address);
    }

    public void AddPlanned(LdapNodeData ldapNodeData, LdapNodeDescription current)
    {
        var urlForDn = _currentTree.GetUrlForDn(ldapNodeData.Dn);
       if (!(passedDns.Contains(urlForDn) || plannedNodes.Any(x=>x.Url.Address == urlForDn)))
       {
           var currentAsParent = current.AddChild(ldapNodeData.Dn, ldapNodeData.Attributes);
       
           plannedNodes.Add(currentAsParent);
       }
    }

    public bool MoveNext()
    {
        if (_current != null)
            Passed(_current);
        if (!plannedNodes.Any())
        {
            _current = null;
            return false;
        }
        _current = plannedNodes.First();
        return true;
    }

    public LdapNodeDescription? Current => _current;
    
}