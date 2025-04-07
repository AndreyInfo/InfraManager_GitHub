using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager;
using Inframanager.BLL;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.MaterialConsumptionRates;

public class MaterialConsumptionRatePermissions:IValidatePermissions<MaterialConsumptionRate>,
    ISelfRegisteredService<IValidatePermissions<MaterialConsumptionRate>>
{
    public Task<bool> UserHasPermissionAsync(Guid userId, ObjectAction action, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}