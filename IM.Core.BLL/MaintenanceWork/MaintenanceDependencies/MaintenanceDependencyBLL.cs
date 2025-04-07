using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.MaintenanceWork;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL.AccessManagement;
using Inframanager;

namespace InfraManager.BLL.MaintenanceWork.MaintenanceDependencies;

internal class MaintenanceDependencyBLL : IMaintenanceDependencyBLL
    , ISelfRegisteredService<IMaintenanceDependencyBLL>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<MaintenanceDependency> _repository;
    private readonly IValidatePermissions<MaintenanceDependency> _validatePermissionsMaintenance;
    private readonly ILogger<MaintenanceDependencyBLL> _logger;
    private readonly ICurrentUser _currentUser;
    private readonly IGuidePaggingFacade<MaintenanceDependency, MaintenanceDependencyListItem> _guidePaggingFacade;

    public MaintenanceDependencyBLL(IMapper mapper
        , IUnitOfWork unitOfWork
        , IRepository<MaintenanceDependency> repository
        , IValidatePermissions<MaintenanceDependency> validatePermissionsMaintenance
        , ILogger<MaintenanceDependencyBLL> logger
        , ICurrentUser currentUser
        , IGuidePaggingFacade<MaintenanceDependency, MaintenanceDependencyListItem> guidePaggingFacade)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _validatePermissionsMaintenance = validatePermissionsMaintenance;
        _logger = logger;
        _currentUser = currentUser;
        _guidePaggingFacade = guidePaggingFacade;
    }

    public async Task<MaintenanceDependencyDetails[]> GetByMaintenanceIDAsync(MaintenanceDependencyFilter filter,
        CancellationToken cancellationToken)
    {
        await CheckAccessAsync(ObjectAction.ViewDetailsArray, cancellationToken);

        var query = _repository.Query().Where(p => p.MaintenanceID == filter.MaintenanceID);
        var maintenanceDependencies = await _guidePaggingFacade.GetPaggingAsync(filter
            , query
            , p => p.ObjectName.ToLower().Contains(filter.SearchString.ToLower())
            , cancellationToken);

        return _mapper.Map<MaintenanceDependencyDetails[]>(maintenanceDependencies);
    }

    public async Task<Guid> AddAsync(MaintenanceDependencyData data, CancellationToken cancellationToken = default)
    {
        await CheckAccessAsync(ObjectAction.Insert, cancellationToken);

        var maintenanceDependency = _mapper.Map<MaintenanceDependency>(data);
        _repository.Insert(maintenanceDependency);
        await _unitOfWork.SaveAsync(cancellationToken);

        _logger.LogTrace($"UserID = {_currentUser.UserId} finish insert a {nameof(MaintenanceDependency)}");
        return maintenanceDependency.MaintenanceID;
    }

    public async Task<Guid> UpdateAsync(MaintenanceDependencyData data, CancellationToken cancellationToken = default)
    {
        await CheckAccessAsync(ObjectAction.Update, cancellationToken);

        var item = await _repository.FirstOrDefaultAsync(p => p.MaintenanceID == data.MaintenanceID
                                                              && p.ObjectID == data.ObjectID
                                                              , cancellationToken);
        _mapper.Map(data, item);
        await _unitOfWork.SaveAsync(cancellationToken);

        _logger.LogTrace($"UserID = {_currentUser.UserId} finish update a {nameof(MaintenanceDependency)}");
        return data.MaintenanceID;
    }

    public async Task DeleteAsync(MaintenanceDependencyDeleteKey key, CancellationToken cancellationToken = default)
    {
        await CheckAccessAsync(ObjectAction.Delete, cancellationToken);

        var entity = await _repository.FirstOrDefaultAsync(c => c.MaintenanceID == key.MaintenanceID
                                                         && c.ObjectID == key.ObjectID
                                                         , cancellationToken)
            ?? throw new ObjectNotFoundException($"Not found {nameof(MaintenanceDependency)} with MaintenanceID = '{key.MaintenanceID}' and ObjectID = '{key.ObjectID}'"); 

        _repository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        _logger.LogTrace($"UserID = {_currentUser.UserId} finish delete a {nameof(MaintenanceDependency)}");
    }

    private async Task CheckAccessAsync(ObjectAction action, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} requested {action} a {nameof(MaintenanceDependency)}");
        await _validatePermissionsMaintenance.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, action, cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} start {action} a {nameof(MaintenanceDependency)}");
    }
}
