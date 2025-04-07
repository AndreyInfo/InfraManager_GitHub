using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import;

public class UIADConfigurationRemoveQuery:IRemoveQuery<Guid,UIADConfiguration>
{
    private readonly IFinderQuery<Guid, UIADConfiguration> _finderQuery;
    private readonly IFilterEntity<UIADIMFieldConcordance, UIADIMFieldConcordancesFilter> _filterEntity;
    private readonly IRemoveQuery<UIADIMFieldConcordancesKey, UIADIMFieldConcordance> _concordanceRemoveQuery;
    private readonly IRepository<UIADConfiguration> _configurations;
    private readonly IUnitOfWork _unitOfWork;
    public UIADConfigurationRemoveQuery(IFinderQuery<Guid, UIADConfiguration> finderQuery, 
        IFilterEntity<UIADIMFieldConcordance, UIADIMFieldConcordancesFilter> filterEntity, 
        IRemoveQuery<UIADIMFieldConcordancesKey, UIADIMFieldConcordance> concordanceRemoveQuery, 
        IRepository<UIADConfiguration> configurations, 
        IUnitOfWork unitOfWork)
    {
        _finderQuery = finderQuery;
        _filterEntity = filterEntity;
        _concordanceRemoveQuery = concordanceRemoveQuery;
        _configurations = configurations;
        _unitOfWork = unitOfWork;
    }

    public async Task RemoveAsync(Guid key, CancellationToken token)
    {
        var entity = await _finderQuery.GetFindQueryAsync(key, token);
        var concordanceFilter = new UIADIMFieldConcordancesFilter()
        {
            ConfigurationID = key
        };
        var concordanceKeys = _filterEntity.Query(concordanceFilter).Select(x => new UIADIMFieldConcordancesKey()
            {
                ADClassID = x.ClassID,
                ADConfigurationID = x.ConfigurationID,
                IMFieldID = x.IMFieldID
            })
            .ToArray();
        foreach (var concordanceKey in concordanceKeys)
        {
            await _concordanceRemoveQuery.RemoveAsync(concordanceKey, token);
        }

        await _unitOfWork.SaveAsync(token);

        _configurations.Delete(entity);

        await _unitOfWork.SaveAsync(token);
    }
}