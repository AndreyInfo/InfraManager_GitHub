using Inframanager.BLL;
using InfraManager.DAL.Software;
using Inframanager;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Software.SoftwareModelTabs.Relations;
using InfraManager.BLL.Software.SoftwareModels.ForTable;
using AutoMapper;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Updates;
public class SoftwareModelUpdateBLL : ISoftwareModelUpdateBLL, ISelfRegisteredService<ISoftwareModelUpdateBLL>
{
    private readonly IMapper _mapper;
    private readonly ILogger<SoftwareModelUpdateBLL> _logger;
    private readonly ICurrentUser _currentUser;
    private readonly IValidatePermissions<SoftwareModel> _validatePermissions;
    private readonly ISoftwareModelRelationBLL _softwareModelRelationBLL;
    private readonly IGuidePaggingFacade<SoftwareModel, SoftwareModelForTable> _guidePaggingFacade;

    public SoftwareModelUpdateBLL(
        IMapper mapper,
        ILogger<SoftwareModelUpdateBLL> logger,
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

    public async Task<SoftwareModelUpdateListItemDetails[]> GetUpdatesForSoftwareModelAsync(SoftwareModelTabFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var query = await _softwareModelRelationBLL.GetRelatedSoftwareModelsByIDAndTypeQueryAsync(
            filter.ID, 
            SoftwareModelDependencyType.UpdateCorrection, 
            true, 
            cancellationToken);

        var result = await _guidePaggingFacade.GetPaggingAsync(
            filter,
            query,
            x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
            cancellationToken);

        return _mapper.Map<SoftwareModelUpdateListItemDetails[]>(result);
    }
}
