using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.DAL.MaintenanceWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.MaintenanceWork.Maintenances;

internal class MaintenanceBLL :
    StandardBLL<Guid, Maintenance, MaintenanceData, MaintenanceDetails, MaintenanceFilter>
    , IMaintenanceBLL
    , ISelfRegisteredService<IMaintenanceBLL>
{
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<MaintenanceFolder> _repositoryFolders;
    private readonly IValidatePermissions<Maintenance> _validatePermissionsMaintenance;
    private readonly IClientSideFilterer<MaintenanceDetails, MaintenanceColumns> _guidePaggingFacade;

    public MaintenanceBLL(IRepository<Maintenance> repository
        , ILogger<MaintenanceBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<MaintenanceDetails, Maintenance> detailsBuilder
        , IInsertEntityBLL<Maintenance, MaintenanceData> insertEntityBLL
        , IModifyEntityBLL<Guid, Maintenance, MaintenanceData, MaintenanceDetails> modifyEntityBLL
        , IRemoveEntityBLL<Guid, Maintenance> removeEntityBLL
        , IGetEntityBLL<Guid, Maintenance, MaintenanceDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, Maintenance, MaintenanceDetails, MaintenanceFilter> detailsArrayBLL
        , IMapper mapper
        , IReadonlyRepository<MaintenanceFolder> repositoryFolders
        , IValidatePermissions<Maintenance> validatePermissionsMaintenance
        , IClientSideFilterer<MaintenanceDetails, MaintenanceColumns> guidePaggingFacade)
        : base(repository
            , logger
            , unitOfWork
            , currentUser
            , detailsBuilder
            , insertEntityBLL
            , modifyEntityBLL
            , removeEntityBLL
            , detailsBLL
            , detailsArrayBLL)
    {
        _mapper = mapper;
        _repositoryFolders = repositoryFolders;
        _validatePermissionsMaintenance = validatePermissionsMaintenance;
        _guidePaggingFacade = guidePaggingFacade;
    }

 
    public async Task<MaintenanceDetails[]> GetByFolderIDAsync(MaintenanceFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissionsMaintenance.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var entities = filter.FolderID.HasValue 
            ? await GetMaintenancesFromFolderAsync(filter, cancellationToken)
            : await Repository.With(c=> c.Folder)
                              .With(c=> c.WorkOrderTemplate)
                              .ToArrayAsync(cancellationToken);

        var result = _mapper.Map<MaintenanceDetails[]>(entities);

        return await _guidePaggingFacade.GetPaggingAsync(result, filter
                , c => c.Name.ToLower().Contains(filter.SearchString.ToLower())
                , cancellationToken);
    }

    private async Task<Maintenance[]> GetMaintenancesFromFolderAsync(MaintenanceFilter filter, CancellationToken cancellationToken)
    {
        var isExists = await _repositoryFolders.AnyAsync(c => c.ID == filter.FolderID, cancellationToken);
        if (filter.FolderID.HasValue && !isExists)
            throw new ObjectNotFoundException<Guid>(filter.FolderID.Value, ObjectClass.MaintenanceFolder);

        return await GetChildFolderByIDAsync(filter.FolderID.Value, cancellationToken);
    }

    private async Task<Maintenance[]> GetChildFolderByIDAsync(Guid folderID, CancellationToken cancellationToken)
    {
        var folder = await _repositoryFolders.WithMany(c => c.SubFolders)
                                             .WithMany(c => c.Maintenances)
                                                 .ThenWith(c => c.WorkOrderTemplate)
                                            .FirstOrDefaultAsync(c => c.ID == folderID, cancellationToken);

        var result = new List<Maintenance>();
        var folders = new List<MaintenanceFolder>
        {
            folder
        };

        do
        {
            folder = folders.First();
            result.AddRange(folder.Maintenances);
            folders.AddRange(folder.SubFolders);
            folders.Remove(folder);
        }
        while (folders.Any());

        return result.ToArray();

    }
}
