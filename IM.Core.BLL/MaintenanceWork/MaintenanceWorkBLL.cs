using InfraManager.DAL.MaintenanceWork;
using InfraManager.DAL;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using Inframanager;

namespace InfraManager.BLL.MaintenanceWork;

internal sealed class MaintenanceWorkBLL : IMaintenanceWorkBLL
    , ISelfRegisteredService<IMaintenanceWorkBLL>
{
    private readonly IMapper _mapper;
    private readonly ILogger<MaintenanceWorkBLL> _logger;
    private readonly ICurrentUser _currentUser;
    private readonly IValidatePermissions<MaintenanceFolder> _validateObjectPermissions;
    private readonly IReadonlyRepository<MaintenanceFolder> _repository;
    private readonly IReadonlyRepository<Maintenance> _maintenanceRepository;
    private readonly IServiceMapper<ObjectClass, IMaintenanceNodeTreeQuery> _serviceMapper;

    public MaintenanceWorkBLL(
        IMapper mapper
        , ILogger<MaintenanceWorkBLL> logger
        , ICurrentUser currentUser
        , IValidatePermissions<MaintenanceFolder> validateObjectPermissions
        , IReadonlyRepository<MaintenanceFolder> repository
        , IReadonlyRepository<Maintenance> maintenanceRepository
        , IServiceMapper<ObjectClass, IMaintenanceNodeTreeQuery> serviceMapper)
    {
        _mapper = mapper;
        _logger = logger;
        _currentUser = currentUser;
        _validateObjectPermissions = validateObjectPermissions;
        _repository = repository;
        _maintenanceRepository = maintenanceRepository;
        _serviceMapper = serviceMapper;

    }

    public async Task<MaintenanceNodeTreeDetails[]> GetFolderTreeAsync(Guid? parentID, CancellationToken cancellationToken = default)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} requested a tree of '{nameof(Maintenance)}' and '{nameof(MaintenanceFolder)}'");
        
        await _validateObjectPermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);
        
        _logger.LogTrace($"UserID = {_currentUser.UserId} start getting node for tree of '{nameof(Maintenance)}' and '{nameof(MaintenanceFolder)}'");

        var nodes = new List<MaintenanceNodeTree>();

        nodes.AddRange(
            await _serviceMapper.Map(ObjectClass.MaintenanceFolder)
                                .ExecuteAsync(parentID,cancellationToken));

        if (parentID.HasValue)
        {
            _logger.LogTrace($"UserID = {_currentUser.UserId} start getting nodes of '{nameof(Maintenance)}' for tree ");

            nodes.AddRange(
           await _serviceMapper.Map(ObjectClass.Maintenance)
                               .ExecuteAsync(parentID, cancellationToken));

            _logger.LogTrace($"UserID = {_currentUser.UserId} finish getting nodes of '{nameof(Maintenance)}' for tree ");
        }
            

        _logger.LogTrace($"UserID = {_currentUser.UserId} finish getting node for tree of '{nameof(Maintenance)}' and '{nameof(MaintenanceFolder)}'");
        return _mapper.Map<MaintenanceNodeTreeDetails[]>(nodes);

    }

    public async Task<MaintenanceNodeTreeDetails[]> GetPathToElementAsync(Guid id, ObjectClass classID, CancellationToken cancellationToken = default)
    {
        _logger.LogTrace(
            $"UserID = {_currentUser.UserId} requested a path from tree of '{nameof(Maintenance)}' and '{nameof(MaintenanceFolder)}' for node with ID = {id} and ClassID = {classID}");
        
        await _validateObjectPermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId,
            ObjectAction.ViewDetailsArray, cancellationToken);
        
        _logger.LogTrace(
            $"UserID = {_currentUser.UserId} start a path from tree of '{nameof(Maintenance)}' and '{nameof(MaintenanceFolder)}' for node with ID = {id} and ClassID = {classID}");

        if (classID == ObjectClass.MaintenanceFolder)
        {
            return await GetPathFolderByIDAsync(id, cancellationToken);
        }

        if (classID == ObjectClass.Maintenance)
        {
            return await GetPathMaintenanceByIDAsync(id, cancellationToken);
        }

        throw new ArgumentException($"Not corrected classID = '{classID}'");
    }

    private async Task<MaintenanceNodeTreeDetails[]> GetPathMaintenanceByIDAsync(Guid id, CancellationToken cancellationToken)
    {
        var maintenance = await _maintenanceRepository.With(c => c.Folder)
            .FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Maintenance);

        var result = new List<MaintenanceNodeTreeDetails>()
        {
            _mapper.Map<MaintenanceNodeTreeDetails>(maintenance)
        };

        result.AddRange(await GetPathFolderByIDAsync(maintenance.FolderID.Value, cancellationToken));

        return result.ToArray();
    }

    private async Task<MaintenanceNodeTreeDetails[]> GetPathFolderByIDAsync(Guid id, CancellationToken cancellationToken)
    {
        var folder = await _repository.With(c => c.Parent)
            .FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.MaintenanceFolder);

        var queue = new Queue<MaintenanceNodeTreeDetails>();

        do
        {
            queue.Enqueue(_mapper.Map<MaintenanceNodeTreeDetails>(folder));
            folder = folder.Parent;
        } while (folder is not null);

        return queue.ToArray();
    }
}
