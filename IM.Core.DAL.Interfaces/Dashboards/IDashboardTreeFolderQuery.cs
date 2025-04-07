using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Dashboards;

// <summary>
/// Получить список папок панелей, которые доступны пользователю
/// </summary>
public interface IDashboardTreeFolderQuery
{
	/// <summary>
	/// Получить список папок панелей, которые доступны пользователю
	/// </summary>
	/// <param name="userID">id пользователя</param>
	Task<DashboardTreeResultItem[]> ExecuteAsync(Guid userID, CancellationToken cancellationToken = default);
}

