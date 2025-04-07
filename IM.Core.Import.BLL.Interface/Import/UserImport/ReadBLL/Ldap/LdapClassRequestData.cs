namespace IM.Core.Import.BLL.Import;

public record LdapClassRequestData
{
    private readonly HashSet<string> _attributes = new();

    public IEnumerable<string> GetAttributes() => _attributes;

    public void AddAttributes(IEnumerable<string> attributes) => _attributes.UnionWith(attributes);

    public void AddAttribute(string attribute) => _attributes.Add(attribute);
}