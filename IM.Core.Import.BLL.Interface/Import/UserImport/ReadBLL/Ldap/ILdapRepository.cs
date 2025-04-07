using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface.Import;

namespace IM.Core.Import.BLL.Interface.Ldap;

public interface ILdapRepository
{
    IEnumerable<ILdapDataEntry> GetAllAttributes(string url, string login, string password,
        ParsersData parserData,
        LdapLoadRequestData ldapLoadRequestData = null);
}