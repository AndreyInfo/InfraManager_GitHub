
namespace IM.Core.Import.BLL.Ldap;

public class IMLdapForestContext
{
    private readonly HashSet<string> plannedAddresses = new();
    private readonly List<IMLdapTreeContext> _forest = new();
    private string _current;
    private IMLdapTreeContext _currentContext;

    public IMLdapTreeContext Current => _currentContext;
    public string CurrentAddress => _current;
    public void AddAddress(string[] urls)
    {
        foreach (var url in urls)
        {
            if (IsPassed(url))
                return;
            var ldapUrlData = new LdapUrlData(url);
            var address = ldapUrlData.Address;
            plannedAddresses.Add(address);
        }
    }

    private bool IsPassed(string url)
    {
        return _forest.Any(x=>x.IsReturnedOrPlanned(url));
    }

    public bool MoveNext()
    {
        plannedAddresses.RemoveWhere(IsPassed);
        if (_current != null)
            plannedAddresses.Remove(_current);
        
        if (!plannedAddresses.Any())
        {
            _current = null;
            _currentContext = null;
            return false;
        }

        _current = plannedAddresses.First();
        var context = _forest.FirstOrDefault(x => x.HasParent(_current));
        if (context != null)
        {
            _currentContext = context;
            return true;
        }

        var treeContext = new IMLdapTreeContext(_current, this);
        _forest.Add(treeContext);
        _currentContext = treeContext;
        return true;
    }

    public void AddAddress(string url)
    {
        AddAddress(new []{url});
    }
}