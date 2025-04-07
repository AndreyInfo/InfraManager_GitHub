using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles;

internal sealed class LifeCycleCatalogBLL : ILifeCycleCatalogBLL
    , ISelfRegisteredService<ILifeCycleCatalogBLL>
{
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<LifeCycleCatalogBLL> _logger;
    private readonly IValidatePermissions<LifeCycle> _validatePermissions;
    private readonly IServiceMapper<ObjectClass, ILifeCycleNodeQuery> _serviceMapping;
    public LifeCycleCatalogBLL(ICurrentUser currentUser
        , ILogger<LifeCycleCatalogBLL> logger
        , IValidatePermissions<LifeCycle> validatePermissions
        , IServiceMapper<ObjectClass, ILifeCycleNodeQuery> serviceMapping)
    {
        _currentUser = currentUser;
        _logger = logger;
        _validatePermissions = validatePermissions;
        _serviceMapping = serviceMapping;
    }

    public async Task<LifeCycleTreeNode[]> GetNodesAsync(LifeCycleTreeFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        return await _serviceMapping.Map(filter.ClassID)
            .ExecuteAsync(filter.ParentID, filter.RoleID, cancellationToken);
    }
}
