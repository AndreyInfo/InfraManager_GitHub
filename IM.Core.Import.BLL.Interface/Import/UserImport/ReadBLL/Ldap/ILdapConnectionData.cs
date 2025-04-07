namespace IM.Core.Import.BLL.Interface.Ldap;

public interface ILdapConnectionData
{
    string HostName { get; init; }
    int Port { get; init; }
}