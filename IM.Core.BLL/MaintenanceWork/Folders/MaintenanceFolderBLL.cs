using InfraManager.DAL.MaintenanceWork;
using System;
using InfraManager.DAL;
using Inframanager.BLL;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.MaintenanceWork.Folders;

internal sealed class MaintenanceFolderBLL :
    StandardBLL<Guid, MaintenanceFolder, MaintenanceFolderData, MaintenanceFolderDetails, MaintenanceFolderFilter>
    , IMaintenanceFolderBLL
    , ISelfRegisteredService<IMaintenanceFolderBLL>
{
    public MaintenanceFolderBLL(IRepository<MaintenanceFolder> repository
        , ILogger<MaintenanceFolderBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<MaintenanceFolderDetails, MaintenanceFolder> detailsBuilder
        , IInsertEntityBLL<MaintenanceFolder, MaintenanceFolderData> insertEntityBLL
        , IModifyEntityBLL<Guid, MaintenanceFolder, MaintenanceFolderData, MaintenanceFolderDetails> modifyEntityBLL
        , IRemoveEntityBLL<Guid, MaintenanceFolder> removeEntityBLL
        , IGetEntityBLL<Guid, MaintenanceFolder, MaintenanceFolderDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, MaintenanceFolder, MaintenanceFolderDetails, MaintenanceFolderFilter> detailsArrayBLL) 
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
    }
}
