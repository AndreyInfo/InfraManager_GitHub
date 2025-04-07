using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Dashboards;

// <summary>
/// Получить список панелей, которые доступны пользователю
/// </summary>
public interface IDashboardTreeQuery
{
	/// <summary>
	/// Получить список панелей, которые доступны пользователю
	/// </summary>
	/// <param name="classId">Класс панели</param>
	/// <param name="userID">id пользователя</param>
	Task<DashboardTreeResultItem[]> ExecuteAsync(ObjectClass classID, Guid userID, CancellationToken cancellationToken = default);
}