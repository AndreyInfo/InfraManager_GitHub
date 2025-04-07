using Novell.Directory.Ldap;

namespace IM.Core.Import.BLL.Ldap;

public class LdapUrlData
{
    private readonly LdapUrl _url;

    public LdapUrlData(string url)
    {
        _url = new LdapUrl(url);
    }

    public string GetUrlForDn(string dn)
    {
        var newAddress = new LdapUrl(_url.Host, _url.Port, dn);
        var otherDnAddress = newAddress.ToString();
        return otherDnAddress;
    }

    public string Address => _url.ToString();

    public LdapUrlData GetOtherDnUrlData(string dn)
    {
        return new LdapUrlData(GetUrlForDn(dn));
    }
    
    public LdapUrl GetOtherDnUrl(string dn)
    {
        var urlAttributeArray = _url.AttributeArray??Array.Empty<string>();
        var urlFilter = _url.Filter ?? string.Empty;
        var urlExtensions = _url.Extensions?? Array.Empty<string>();
        var newAddress = new LdapUrl(_url.Host, _url.Port, dn, urlAttributeArray, _url.Scope, urlFilter,
            urlExtensions, _url.Secure);
        return newAddress;
    }

    public string Dn => _url.GetDn();
}