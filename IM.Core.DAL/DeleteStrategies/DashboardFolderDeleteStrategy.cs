using InfraManager.DAL.Dashboards;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.DeleteStrategies;

internal class DashboardFolderDeleteStrategy : IDeleteStrategy<DashboardFolder>, ISelfRegisteredService<IDeleteStrategy<DashboardFolder>>
{
    private readonly IRepository<Dashboard> _dashboardRepository;
    private readonly DbSet<DashboardFolder> _dashboardFolders;

    public DashboardFolderDeleteStrategy(IRepository<Dashboard> dashboardRepository,
        DbSet<DashboardFolder> dashboardFolders)
    {
        _dashboardRepository = dashboardRepository;
        _dashboardFolders = dashboardFolders;
    }

    public void Delete(DashboardFolder entity)
    {
        DeleteDashboards(entity);
        DeleteChildFolders(entity);
        _dashboardFolders.Remove(entity);
    }

    private void DeleteChildFolders(DashboardFolder entity)
    {
        var childs = _dashboardFolders.Where(x => x.ParentDashboardFolderID == entity.ID).ToList();
        foreach (var child in childs)
        {
            DeleteChildFolders(child);
            _dashboardFolders.Remove(child);
        }
    }

    private void DeleteDashboards(DashboardFolder entity)
    {
        List<Dashboard> dashboardsForDelete = new List<Dashboard>();
        dashboardsForDelete.AddRange(_dashboardRepository.Where(x => x.DashboardFolderID == entity.ID));

        dashboardsForDelete.AddRange(AddDeleteDashboards(entity.ID));

        foreach (var dashboard in dashboardsForDelete)
        {
            _dashboardRepository.Delete(dashboard);
        }
    }

    private IEnumerable<Dashboard> AddDeleteDashboards(Guid? iD)
    {
        List<Dashboard> deleteDashboards = new List<Dashboard>();
        var folders = _dashboardFolders.Where(x => x.ParentDashboardFolderID == iD).ToList();
        foreach (var folder in folders)
        {
            deleteDashboards.AddRange(AddDeleteDashboards(folder.ID));
            var dashboards = _dashboardRepository.Where(x => x.DashboardFolderID == folder.ID);
            deleteDashboards.AddRange(dashboards);
        }
        return deleteDashboards;
    }
}
