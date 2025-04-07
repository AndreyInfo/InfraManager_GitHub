using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Dashboards;

/// <summary>
/// Получить данные для отображения панели
/// </summary>
public interface IDevExpressDashboardQuery
{
    /// <summary>
    /// Получить данные для отображения панели
    /// </summary>
    /// <param name="dashboardID"></param>
    /// <returns></returns>
    Task<string> ExecuteAsync(Guid dashboardID, CancellationToken cancellationToken = default);
}