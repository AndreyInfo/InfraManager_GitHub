using Inframanager.BLL;
using Inframanager;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using InfraManager.DAL.Software;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Installations;
public class SoftwareModelInstallationBLL : ISoftwareModelInstallationBLL, ISelfRegisteredService<ISoftwareModelInstallationBLL>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<SoftwareModelInstallationBLL> _logger;
    private readonly IValidatePermissions<SoftwareModel> _validatePermissions;
    private readonly IRepository<SoftwareInstallation> _softwareInstallationsRepository;
    private readonly IGuidePaggingFacade<SoftwareInstallation, SoftwareModelInstallationForTable> _guidePaggingFacade;

    public SoftwareModelInstallationBLL(
        IMapper mapper, 
        ICurrentUser currentUser, 
        ILogger<SoftwareModelInstallationBLL> logger, 
        IValidatePermissions<SoftwareModel> validatePermissions,
        IRepository<SoftwareInstallation> softwareInstallationsRepository,
        IGuidePaggingFacade<SoftwareInstallation, SoftwareModelInstallationForTable> guidePaggingFacade
        )
    {
        _mapper = mapper;
        _currentUser = currentUser;
        _logger = logger;
        _validatePermissions = validatePermissions;
        _softwareInstallationsRepository = softwareInstallationsRepository;
        _guidePaggingFacade = guidePaggingFacade;
    }

    public async Task<SoftwareModelInstallationListItemDetails[]> GetInstallationsForSoftwareModelAsync(SoftwareModelTabFilter filter, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var query = _softwareInstallationsRepository.Query().Where(x => x.SoftwareModelID == filter.ID);

        var result = await _guidePaggingFacade.GetPaggingAsync(
            filter,
            query,
            x => x.SoftwareModel.Name.ToLower().Contains(filter.SearchString.ToLower()),
            cancellationToken);

        return _mapper.Map<SoftwareModelInstallationListItemDetails[]>(result);
    }
}
