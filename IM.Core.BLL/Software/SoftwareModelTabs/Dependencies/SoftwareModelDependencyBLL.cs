using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Software.SoftwareModels;
using InfraManager.BLL.Software.SoftwareModels.ForTable;
using InfraManager.BLL.Software.SoftwareModelTabs.Relations;
using InfraManager.DAL.Software;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Dependencies;
internal class SoftwareModelDependencyBLL : ISoftwareModelDependencyBLL, ISelfRegisteredService<ISoftwareModelDependencyBLL>
{
    private readonly IMapper _mapper;
    private readonly ILogger<SoftwareModelDependencyBLL> _logger;
    private readonly ICurrentUser _currentUser;
    private readonly IValidatePermissions<SoftwareModel> _validatePermissions;
    private readonly ISoftwareModelRelationBLL _softwareModelRelationBLL;
    private readonly IGuidePaggingFacade<SoftwareModel, SoftwareModelForTable> _guidePaggingFacade;

    public SoftwareModelDependencyBLL(
        IMapper mapper,
        ILogger<SoftwareModelDependencyBLL> logger,
        ICurrentUser currentUser,
        IValidatePermissions<SoftwareModel> validatePermissions,
        ISoftwareModelRelationBLL softwareModelRelationBLL,
        IGuidePaggingFacade<SoftwareModel, SoftwareModelForTable> guidePaggingFacade
        )
    {
        _mapper = mapper;
        _logger = logger;
        _currentUser = currentUser;
        _validatePermissions = validatePermissions;
        _softwareModelRelationBLL = softwareModelRelationBLL;
        _guidePaggingFacade = guidePaggingFacade;
    }

    public async Task<SoftwareModelListItemDetails[]> GetDependenciesForSoftwareModelAsync(SoftwareModelTabDependencyFilter filter, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var query = await _softwareModelRelationBLL.GetRelatedSoftwareModelsByIDAndTypeQueryAsync(filter.ID, filter.Type, false, cancellationToken);

        query = query.Where(x => x.ID != filter.ID);

        var result = await _guidePaggingFacade.GetPaggingAsync(
            filter,
            query,
            x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
            cancellationToken);

        return _mapper.Map<SoftwareModelListItemDetails[]>(result);
    }
}
