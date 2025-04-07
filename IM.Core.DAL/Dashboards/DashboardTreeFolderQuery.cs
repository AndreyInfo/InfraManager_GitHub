using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Dashboards;


internal class DashboardTreeFolderQuery : IDashboardTreeFolderQuery, ISelfRegisteredService<IDashboardTreeFolderQuery>
{
	private readonly DbContext _db;

	public DashboardTreeFolderQuery(
		CrossPlatformDbContext db)
	{
		_db = db;
	}

	public async Task<DashboardTreeResultItem[]> ExecuteAsync(Guid userID, CancellationToken cancellationToken = default)
	{
		var result = await
			(from df in _db.Set<DashboardFolder>().AsNoTracking()
			 where Dashboard.DbFuncDashboardTreeItemIsVisible(ObjectClass.DashboardFolder, df.ID, userID)
			 select new DashboardTreeResultItem
			 {
				 ID = df.ID,
				 ParentFolderID = df.ParentDashboardFolderID,
				 Name = df.Name,
				 HasChilds = true,
				 IsDashboard = false,
			 }).ToArrayAsync(cancellationToken);

		return result;
	}
}

