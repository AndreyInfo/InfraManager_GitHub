using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Dashboards;


internal class DashboardTreeQuery : IDashboardTreeQuery, ISelfRegisteredService<IDashboardTreeQuery>
{
	private readonly DbContext _db;

	public DashboardTreeQuery(
		CrossPlatformDbContext db)
	{
		_db = db;
	}

	public async Task<DashboardTreeResultItem[]> ExecuteAsync(ObjectClass classID, Guid userID, CancellationToken cancellationToken = default)
	{
		var result = await
			(from d in _db.Set<Dashboard>().AsNoTracking()
			 where d.ObjectClassID == classID
				 && Dashboard.DbFuncDashboardTreeItemIsVisible(classID, d.ID, userID)
			 select new DashboardTreeResultItem
			 {
				 ID = d.ID,
				 ParentFolderID = d.DashboardFolderID,
				 Name = d.Name,
				 HasChilds = false,
				 IsDashboard = true,
			 }).ToArrayAsync(cancellationToken);

		return result;
	}
}

