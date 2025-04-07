using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Software.SoftwareModels.ForTable;
using InfraManager.BLL.Software.SoftwareModelTabs.Dependencies;
using InfraManager.DAL;
using InfraManager.DAL.Software;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Relations;
internal class SoftwareModelRelationBLL : ISoftwareModelRelationBLL, ISelfRegisteredService<ISoftwareModelRelationBLL>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<SoftwareModelRelationBLL> _logger;
    private readonly IValidatePermissions<SoftwareModel> _validatePermissions;
    private readonly IRepository<SoftwareModel> _softwareModelsRepository;
    private readonly IRepository<SoftwareModelDependency> _softwareModelDependenciesRepository;
    private readonly IGuidePaggingFacade<SoftwareModel, SoftwareModelForTable> _guidePaggingFacade;

    public SoftwareModelRelationBLL(
        IMapper mapper,
        ICurrentUser currentUser,
        ILogger<SoftwareModelRelationBLL> logger,
        IValidatePermissions<SoftwareModel> validatePermissions,
        IRepository<SoftwareModel> softwareModelsRepository,
        IRepository<SoftwareModelDependency> softwareModelDependenciesRepository,
        IGuidePaggingFacade<SoftwareModel, SoftwareModelForTable> guidePaggingFacade
        )
    {
        _mapper = mapper;
        _currentUser = currentUser;
        _logger = logger;
        _validatePermissions = validatePermissions;
        _softwareModelsRepository = softwareModelsRepository;
        _softwareModelDependenciesRepository = softwareModelDependenciesRepository;
        _guidePaggingFacade = guidePaggingFacade;
    }

    public async Task<SoftwareModelRelatedListItemDetails[]> GetRelationsForSoftwareModelAsync(SoftwareModelTabDependencyFilter filter, CancellationToken cancellationToken = default)
    {
        var query = await GetRelatedSoftwareModelsByIDAndTypeQueryAsync(filter.ID, filter.Type, true, cancellationToken);

        var result = await _guidePaggingFacade.GetPaggingAsync(
            filter,
            query,
            x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
            cancellationToken);

        return _mapper.Map<SoftwareModelRelatedListItemDetails[]>(result);
    }

    public async Task<IExecutableQuery<SoftwareModel>> GetRelatedSoftwareModelsByIDAndTypeQueryAsync(
        Guid? softwareModelID,
        SoftwareModelDependencyType type,
        bool isRelated = true,
        CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var softwareRelations = await _softwareModelDependenciesRepository
            .ToArrayAsync(x => x.ParentSoftwareModelID == softwareModelID && x.Type == type, cancellationToken);

        var childIDs = softwareRelations.Select(x => x.ChildSoftwareModelID);

        var softwareModelsQuery = _softwareModelsRepository.Query();

        if (isRelated)
        {
            softwareModelsQuery.Where(x => childIDs.Contains(x.ID));
        }
        else
        {
            if (softwareModelID.HasValue)
            {
                softwareModelsQuery.Where(x => !childIDs.Contains(x.ID));
            }
            softwareModelsQuery.Where(x => x.Template == (SoftwareModelTemplate)type);
        }

        return softwareModelsQuery;
    }
}
