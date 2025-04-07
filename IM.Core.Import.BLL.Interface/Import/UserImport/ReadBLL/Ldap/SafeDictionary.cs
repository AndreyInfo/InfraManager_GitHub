using System.Collections;

namespace IM.Core.Import.BLL.Interface.Ldap;

public class SafeDictionary:IReadOnlyDictionary<string,string>
{
    private readonly IReadOnlyDictionary<string, string> parent;

    public SafeDictionary(IReadOnlyDictionary<string, string> parent)
    {
        this.parent = parent;
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return parent.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => parent.Count;

    public bool ContainsKey(string key)
    {
       return parent.ContainsKey(key);
    }

    public bool TryGetValue(string key, out string value)
    {
        return parent.TryGetValue(key, out value);
    }

    public string this[string key]
    {
        get
        {
            if (!parent.TryGetValue(key, out var value))
                return default;
            return value;
        }
        
    }

    public IEnumerable<string> Keys => parent.Keys;

    public IEnumerable<string> Values => parent.Values;
}