using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Dashboards;

/// <summary>
/// Поведение для получения данных для панелей статистики
/// </summary>
public interface IGetDashboard
{
    /// <summary>
    /// Получить данные панели
    /// </summary>
    /// <param name="dashboardID">id панели статистики</param>
    Task<string> GetAsync(Guid dashboardID, CancellationToken cancellationToken = default);
}