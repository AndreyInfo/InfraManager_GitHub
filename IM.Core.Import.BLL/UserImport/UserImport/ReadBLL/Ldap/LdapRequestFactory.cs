using System.Text;
using IM.Core.Import.BLL.Interface.Ldap;
using InfraManager;
using Microsoft.Extensions.Configuration;
using Novell.Directory.Ldap;
using Novell.Directory.Ldap.Utilclass;

namespace IM.Core.Import.BLL.Import.Ldap;

public class LdapRequestFactory : ILdapRequestFactory, ISelfRegisteredService<ILdapRequestFactory>
{
    private const int DefaultLDAPPort = 389;
    private readonly IConfiguration _configuration;

    public LdapRequestFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetUserStrings(string[] searchClasses)
    {
        var entry = searchClasses.Aggregate(new StringBuilder(), (a, c) => a.Append($"(objectClass={c})"),
            a => a.ToString());

        var entryData = searchClasses.Length>1?$"(|{entry})":$"{entry}";
        
        return string.IsNullOrWhiteSpace(entry)? "(ObjectClass=*)" : $"(&{entryData}(!(objectClass=computer)))";
    }
    

    public ILdapConnectionData GetConnectionData(string hostName, int? port = null)
    {
        var resultPort = port ?? DefaultLDAPPort;
        return new LdapConnectionData(hostName, resultPort);
    }

    public ILdapRequestData GetRootRequest(ILdapConnectionData connectionData)
    {
        var bindData = new BindData(null, null, LdapConnection.LdapV3);

        var pageSize = GetPageSize();

        var searchData = new SearchData("(objectClass=*)", LdapConnection.ScopeBase, string.Empty, pageSize);
        
        return new LdapRequestData(connectionData, bindData, searchData, GetLdapUrl(connectionData.HostName,connectionData.Port, string.Empty));
    }

    private int? GetPageSize()
    {
        var pageSizeString = _configuration["Ldap:PageSize"];

        int? pageSize;

        if (pageSizeString == null)
            pageSize = null;
        else
        {
            pageSize = int.Parse(pageSizeString);

            if (pageSize <= 0 || pageSize >= 1000)
                throw new ArgumentOutOfRangeException($"Размер страницы указаyн вне диапазона 1..1000 {pageSize}");
        }

        return pageSize;
    }

    public IBindData GetBindData(string dn, string password, int version)
    {
        return new BindData(dn, password, version);
    }

    public ILdapRequestData GetSearchData(ILdapConnectionData connection, IBindData bindData, string baseNode,
        params string[] searchClasses)
    {
        var searchString = GetUserStrings(searchClasses);
        return GetSearchData(connection, bindData, baseNode, searchString, LdapConnection.ScopeOne);
    }

    private static string GetLdapUrl(string host, int port, string baseNode) => $"LDAP://{host}:{port}/{baseNode}";

    private ILdapRequestData GetSearchData(ILdapConnectionData connectionData, IBindData bindData, string baseNode,
        string filter, int scope = LdapConnection.ScopeSub)
    {
        var searchData = new SearchData(filter, scope, baseNode);

        return new LdapRequestData(connectionData, bindData, searchData, GetLdapUrl(connectionData.HostName, connectionData.Port, baseNode));
    }

    public ILdapRequestData GetRootRequest(string path)
    {
        var url = new LdapUrl(path);
        var connection = GetConnectionData(url.Host, url.Port);
        return GetRootRequest(connection);
    }

    public ILdapRequestData GetSearchClassData(ILdapRequestData data, string baseNode, params string[] searchClasses)
    {
        return GetSearchData(data.ConnectionData, data.BindData, baseNode, searchClasses);
    }

    public ILdapRequestData GetSearchClassForDN(ILdapRequestData data, string dn)
    {
        return GetSearchData(data.ConnectionData, data.BindData, dn, filter: $"(objectclass=*)", LdapConnection.ScopeBase);
    }

    public ILdapRequestData GetRequestData(string path, string login, string password, int version, string[] classes)
    {
        var url = new LdapUrl(path);
        var connection = GetConnectionData(url.Host, url.Port);
        var bindData = GetBindData(login, password, version);
        return GetSearchData(connection, bindData, url.GetDn(), classes);
    }
}