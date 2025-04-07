using System.Text;
using Novell.Directory.Ldap;

namespace IM.Core.Import.BLL.Import.Ldap;

public class LdapAuthHandler:ILdapAuthHandler
{
    private readonly LdapAuthProvider _provider;
    public LdapAuthHandler(string dn, string password)
    {
        _provider = new LdapAuthProvider(dn, Encoding.UTF8.GetBytes(password));
    }


    public LdapAuthProvider GetAuthProvider(string host, int port)
    {
        return _provider;
    }
}