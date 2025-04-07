namespace IM.Core.Import.BLL.Interface.Ldap;

public record LdapParents
{
    private readonly List<Dictionary<string, string>> _elements = new();

    public LdapParents()
    {
        
    }

    private LdapParents(List<Dictionary<string, string>> elements)
    {
        _elements = elements;
    }

    public LdapParents(LdapParents parents)
    {
        _elements = parents._elements.ToList();
    }
    
    public void AddParent(Dictionary<string, string> elementData)
    {
        _elements.Add(elementData);
    }

    public List<Dictionary<string, string>> GetParents()
    {
        var parents = _elements.ToList();
        parents.Reverse();
        return parents;
    }

    public LdapParents AddToParents(Dictionary<string, string>? element)
    {
        var subParents = new LdapParents(_elements);
        if (element is not null)
            subParents.AddParent(element);
        return subParents;
    }
}