using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager;
using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk.Quality;

namespace InfraManager.BLL.ProductCatalogue.Slots;

public class QualityControlValidator : IValidatePermissions<QualityControl>,ISelfRegisteredService<IValidatePermissions<QualityControl>>
{
    public Task<bool> UserHasPermissionAsync(Guid userId, ObjectAction action, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}