using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL.Software;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Software.SoftwareModelTabs.Relations;
using InfraManager.BLL.Software.SoftwareModels.ForTable;
using Inframanager;
using Inframanager.BLL.AccessManagement;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Components;
public class SoftwareModelComponentBLL : ISoftwareModelComponentBLL, ISelfRegisteredService<ISoftwareModelComponentBLL>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<SoftwareModelComponentBLL> _logger;
    private readonly IValidatePermissions<SoftwareModel> _validatePermissions;
    private readonly ISoftwareModelRelationBLL _softwareModelRelationBLL;
    private readonly IGuidePaggingFacade<SoftwareModel, SoftwareModelForTable> _guidePaggingFacade;

    public SoftwareModelComponentBLL(
        IMapper mapper, 
        ICurrentUser currentUser, 
        ILogger<SoftwareModelComponentBLL> logger, 
        IValidatePermissions<SoftwareModel> validatePermissions, 
        ISoftwareModelRelationBLL softwareModelRelationBLL,
        IGuidePaggingFacade<SoftwareModel, SoftwareModelForTable> guidePaggingFacade)
    {
        _mapper = mapper;
        _currentUser = currentUser;
        _logger = logger;
        _validatePermissions = validatePermissions;
        _softwareModelRelationBLL = softwareModelRelationBLL;
        _guidePaggingFacade = guidePaggingFacade;
    }

    public async Task<SoftwareModelComponentListItemDetails[]> GetComponentsForSoftwareModelAsync(SoftwareModelTabFilter filter, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var query = await _softwareModelRelationBLL.GetRelatedSoftwareModelsByIDAndTypeQueryAsync(
            filter.ID, 
            SoftwareModelDependencyType.Component, 
            true, 
            cancellationToken);

        var result = await _guidePaggingFacade.GetPaggingAsync(
            filter,
            query,
            x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
            cancellationToken);

        return _mapper.Map<SoftwareModelComponentListItemDetails[]>(result);
    }
}
