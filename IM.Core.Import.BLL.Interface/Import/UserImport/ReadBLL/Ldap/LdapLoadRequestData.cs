namespace IM.Core.Import.BLL.Import;

public class LdapLoadRequestData
{
    private readonly Dictionary<string,LdapClassRequestData> _classData = new();
    private LdapClassRequestData _default = new();
    
    public string[] Classes => _classData.Keys.ToArray();
    public LdapClassRequestData? this[string? name]
    {
        get
        {
            if (name is null)
                return _default;
            if (_classData.ContainsKey(name))
                return _classData[name];
            return null;
        }

        set
        {

            if (name is null)
                _default = value;
            if (_classData.ContainsKey(name))
            {
                if (value == null)
                {
                    _classData.Remove(name);
                    return;
                }
                _classData[name] = value;
            }
            else
                _classData.Add(name,value);
        }
    }
    
    
}