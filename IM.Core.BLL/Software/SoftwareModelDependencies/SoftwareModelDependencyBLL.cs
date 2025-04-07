using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Localization;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Software;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareModelDependencies;

internal class SoftwareModelDependencyBLL : ISoftwareModelDependencyBLL, ISelfRegisteredService<ISoftwareModelDependencyBLL>
{
    private readonly ILogger<SoftwareModelDependencyBLL> _logger;
    private readonly ICurrentUser _currentUser;
    private readonly IInsertEntityBLL<SoftwareModelDependency, SoftwareModelDependencyData> _insertEntityBLL;
    private readonly IRemoveEntityBLL<Guid, SoftwareModelDependency> _removeEntityBLL;
    private readonly IGetEntityArrayBLL<Guid, SoftwareModelDependency, SoftwareModelDependencyDetails, BaseFilter> _detailsArrayBLL;
    private readonly IRepository<SoftwareModel> _softwareModelRepository;
    private readonly IRepository<SoftwareModelDependency> _softwareModelDependencyRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatePermissions<SoftwareModelDependency> _validatePermissions;
    private readonly IGuidePaggingFacade<SoftwareModelDependency, SoftwareModelDependencyForTable> _guidePaggingFacade;
    private readonly ILocalizeText _localizeText;

    public SoftwareModelDependencyBLL(
        ILogger<SoftwareModelDependencyBLL> logger,
        ICurrentUser currentUser,
        IInsertEntityBLL<SoftwareModelDependency, SoftwareModelDependencyData> insertEntityBLL,
        IRemoveEntityBLL<Guid, SoftwareModelDependency> removeEntityBLL,
        IGetEntityArrayBLL<Guid, SoftwareModelDependency, SoftwareModelDependencyDetails, BaseFilter> detailsArrayBLL,
        IRepository<SoftwareModel> softwareModelRepository,
        IRepository<SoftwareModelDependency> softwareModelDependencyRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IValidatePermissions<SoftwareModelDependency> validatePermissions,
        IGuidePaggingFacade<SoftwareModelDependency, SoftwareModelDependencyForTable> guidePaggingFacade,
        ILocalizeText localizeText
        )
    {
        _logger = logger;
        _currentUser = currentUser;
        _insertEntityBLL = insertEntityBLL;
        _removeEntityBLL = removeEntityBLL;
        _detailsArrayBLL = detailsArrayBLL;
        _softwareModelRepository = softwareModelRepository;
        _softwareModelDependencyRepository = softwareModelDependencyRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _validatePermissions = validatePermissions;
        _guidePaggingFacade = guidePaggingFacade;
        _localizeText = localizeText;
    }

    public async Task<SoftwareModelDependencyDetails[]> GetListAsync(SoftwareModelDependencyFilter filter, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var query = _softwareModelDependencyRepository.Query();

        if (filter.softwareModelDependencyType.HasValue)
        {
            query = query.Where(x => x.Type == filter.softwareModelDependencyType);
        }

        var result = await _guidePaggingFacade.GetPaggingAsync(
            filter,
            query,
            x => x.SoftwareModelParent.Name.ToLower().Contains(filter.SearchString.ToLower()),
            cancellationToken);

        return _mapper.Map<SoftwareModelDependencyDetails[]>(result);
    }

    public async Task<SoftwareModelDependencyDetails> AddAsync(SoftwareModelDependencyData model, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);

        var saveModel = await _insertEntityBLL.CreateAsync(model, cancellationToken);

        var updateModel = await _softwareModelRepository
            .FirstOrDefaultAsync(x => x.ID == model.ChildSoftwareModelID)
            ?? throw new ObjectNotFoundException<Guid>(model.ChildSoftwareModelID, ObjectClass.SoftwareModel);

        updateModel.ParentID = model.ParentSoftwareModelID;

        await _unitOfWork.SaveAsync(cancellationToken);
        return _mapper.Map<SoftwareModelDependencyDetails>(saveModel);
    }

    public async Task DeleteAsync(SoftwareModelDependencyData dependencyKey, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

        var deleteModel = await _softwareModelDependencyRepository.FirstOrDefaultAsync(
            x => x.ParentSoftwareModelID == dependencyKey.ParentSoftwareModelID &&
            x.ChildSoftwareModelID == dependencyKey.ChildSoftwareModelID &&
            x.Type == dependencyKey.Type,
            cancellationToken)
            ?? throw new ObjectNotFoundException(await _localizeText.LocalizeAsync(nameof(Resources.SoftwareModelDependencyNotFound), cancellationToken));

        _softwareModelDependencyRepository.Delete(deleteModel);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
