namespace IM.Core.Import.BLL.Interface.Ldap;

public class LdapConnectionData : ILdapConnectionData
{
    public LdapConnectionData(string hostName, int port)
    {
        HostName = hostName;
        Port = port;
    }

    public string HostName { get; init; }

    public int Port { get; init; }
    
}