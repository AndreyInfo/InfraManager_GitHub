using Inframanager.BLL;
using System;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;
using InfraManager.DAL.Dashboards;

namespace InfraManager.BLL.Dashboards;

internal class DashboardFolderBLL :
    StandardBLL<Guid, DashboardFolder, DashboardFolderData, DashboardFolderDetails, DashboardFolderListFilter>,
    IDashboardFolderBLL,
    ISelfRegisteredService<IDashboardFolderBLL>
{

    public DashboardFolderBLL(IRepository<DashboardFolder> repository,
    ILogger<DashboardFolderBLL> logger,
    IUnitOfWork saveChangesCommand,
    ICurrentUser currentUser,
    IBuildObject<DashboardFolderDetails, DashboardFolder> detailsBuilder,
    IInsertEntityBLL<DashboardFolder, DashboardFolderData> insertEntityBLL,
    IModifyEntityBLL<Guid, DashboardFolder, DashboardFolderData, DashboardFolderDetails> modifyEntityBLL,
    IRemoveEntityBLL<Guid, DashboardFolder> removeEntityBLL,
    IGetEntityBLL<Guid, DashboardFolder, DashboardFolderDetails> detailsBLL,
    IGetEntityArrayBLL<Guid, DashboardFolder, DashboardFolderDetails, DashboardFolderListFilter> detailsArrayBLL)
        : base(
        repository,
        logger,
        saveChangesCommand,
        currentUser,
        detailsBuilder,
        insertEntityBLL,
        modifyEntityBLL,
        removeEntityBLL,
        detailsBLL,
        detailsArrayBLL)
    {
    }

}
