using IM.Core.Import.BLL.Ldap;

namespace IM.Core.Import.BLL.Interface.Ldap;

public record LdapNodeDescription
{
    private readonly LdapParents _parentsNodeAttributes;

    private readonly List<LdapNodeDescription> _childs = new();
    
    private readonly LdapNodeDescription? _parentNode;

    public LdapParents Parents => _parentsNodeAttributes;
    
    
    
    public LdapUrlData  Url { get;}
    
    private LdapNodeDescription(LdapUrlData url, LdapParents parentsNodeAttributes, LdapNodeDescription parentNode)
    {
        _parentsNodeAttributes = parentsNodeAttributes;
        Url = url;
        _parentNode = parentNode;
    }
    
    public LdapNodeDescription(LdapUrlData url, LdapParents parentsNodeAttributes)
        :this(url,parentsNodeAttributes, null)
    {
    }
    
    public LdapNodeDescription(LdapUrlData url)
    {
        _parentsNodeAttributes = new LdapParents();
        Url = url;
    }

    public LdapNodeDescription AddChild(string dn, IReadOnlyDictionary<string,string> currentData)
    {
        var dictionaryClone = currentData.ToDictionary(x => x.Key, x => x.Value);
        var parentsClone = new LdapParents(_parentsNodeAttributes);
        var parents = parentsClone.AddToParents(dictionaryClone);
        var ldapNodeDescription = new LdapNodeDescription(Url.GetOtherDnUrlData(dn), parents, this);
        _childs.Add(ldapNodeDescription);
        return ldapNodeDescription;
    }

    public LdapNodeData CreateLdapNode(Dictionary<string,string> current)
    {
        var parents = _parentsNodeAttributes.GetParents().Cast<IReadOnlyDictionary<string,string>>().ToArray();
        var dictionaryClone = current.ToDictionary(x=>x.Key,x=>x.Value);
        return new LdapNodeData(Url.Dn, parents, dictionaryClone)
        {
            Address = Url.Address
        };
    }

    public LdapNodeDescription GetWithOtherUrl(string url)
    {
        var newParents = _parentsNodeAttributes.AddToParents(null);
        return new LdapNodeDescription(Url.GetOtherDnUrlData(url), newParents);
    }

    public LdapNodeDescription? SearchForAddress(LdapUrlData urlData)
    {
         var urlDataAddress = urlData.Address;
        foreach (var child in _childs)
        {
            if (child.Url.Address == urlDataAddress)
                return child;
            var childResult = child.SearchForAddress(urlData);
            if (childResult != null)
                return childResult;
        }

        return null;
    }

    public bool HasParent(string invariantAddress)
    {
        var current = _parentNode;
        while (current != null)
        {
            if (current.Url.Address == invariantAddress)
                return true;
            current = current._parentNode;
        }

        return false;
    }
}