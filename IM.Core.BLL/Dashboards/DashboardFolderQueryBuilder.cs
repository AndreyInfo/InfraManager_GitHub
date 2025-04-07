using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Dashboards;

namespace InfraManager.BLL.Dashboards;

internal class DashboardFolderQueryBuilder :
    IBuildEntityQuery<DashboardFolder, DashboardFolderDetails, DashboardFolderListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<DashboardFolder, DashboardFolderDetails, DashboardFolderListFilter>>
{
    private readonly IReadonlyRepository<DashboardFolder> _repository;

    public DashboardFolderQueryBuilder(IReadonlyRepository<DashboardFolder> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<DashboardFolder> Query(DashboardFolderListFilter filterBy)
    {
        return _repository.Query().Where(x => x.ParentDashboardFolderID == filterBy.ParentFolderID);
    }
}
