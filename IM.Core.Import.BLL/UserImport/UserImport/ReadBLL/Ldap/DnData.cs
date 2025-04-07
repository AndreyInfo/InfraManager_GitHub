using System.Text;

namespace IM.Core.Import.BLL.Ldap;

public class DnData
{
    private readonly string[] _rdns;

    public DnData(string dn)
    {
        _rdns = dn.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Reverse().ToArray();
    }

    public IEnumerable<string> GetParents()
    {
        string current = string.Empty;
        if (_rdns.Any())
        {
            current = _rdns.First();
            yield return current;
        }
        foreach (var rdn in _rdns.Skip(1).Take(_rdns.Length-2))
        {
            current = $"{rdn},{current}";
            yield return current;
        }
    }
}