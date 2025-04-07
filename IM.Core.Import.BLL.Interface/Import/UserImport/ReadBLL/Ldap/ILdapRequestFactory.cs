namespace IM.Core.Import.BLL.Interface.Ldap;

public interface ILdapRequestFactory
{
    ILdapRequestData GetRootRequest(string path);

    ILdapRequestData GetSearchClassData(ILdapRequestData data, string baseNode,
        params string[] searchClasses);

    ILdapRequestData GetSearchClassForDN(ILdapRequestData data, string dn);

    ILdapRequestData GetRequestData(string path, string login, string password, int version, string[] classes);
}