using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Dashboards;

namespace InfraManager.BLL.Dashboards;

internal class DashboardQueryBuilder:
    IBuildEntityQuery<Dashboard, DashboardDetails, DashboardListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<Dashboard, DashboardDetails, DashboardListFilter>>
{
    private readonly IReadonlyRepository<Dashboard> _repository;
    public DashboardQueryBuilder(IReadonlyRepository<Dashboard> repository)
    {
        _repository = repository;
    }
    public IExecutableQuery<Dashboard> Query(DashboardListFilter filterBy)
    {
        var query = _repository.Query();

        if (filterBy.FolderID is not null)
        {
            query = query.Where(x => x.DashboardFolderID == filterBy.FolderID);
        }

        return query;
    }
}
