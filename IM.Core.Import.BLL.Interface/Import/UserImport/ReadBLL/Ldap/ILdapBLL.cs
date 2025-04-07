using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager;

namespace IM.Core.Import.BLL.Interface.Ldap;

public interface ILdapBLL
{
    IEnumerable<ImportModel> GetImportModels(
        string path, 
        string accountName, 
        string password,
        ILookup<string, ScriptDataDetails<ConcordanceObjectType>> userScriptsByClass,
        Func<ImportModel, string> getManagerIdentificator,
        LdapLoadRequestData classes = null);

    bool CheckUniqueDN(string userName, string password, string dn, string path);
}