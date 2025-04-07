using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import;

public class UIADIMFieldConcordanceFinderQuery:IFinderQuery<UIADIMFieldConcordancesKey, UIADIMFieldConcordance>
{
    private readonly IRepository<UIADIMFieldConcordance> _concordances;

    public UIADIMFieldConcordanceFinderQuery(IRepository<UIADIMFieldConcordance> concordances)
    {
        _concordances = concordances;
    }
    
    public Task<UIADIMFieldConcordance> GetFindQueryAsync(UIADIMFieldConcordancesKey key, CancellationToken token)
    {
        return _concordances.SingleOrDefaultAsync(x =>
            x.ClassID == key.ADClassID && x.ConfigurationID == key.ADConfigurationID && x.IMFieldID == key.IMFieldID, token);
    }
}