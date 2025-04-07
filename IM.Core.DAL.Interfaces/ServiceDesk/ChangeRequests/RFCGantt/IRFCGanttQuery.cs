using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Interface.ServiceDesk.ChangeRequests.RFCGantt;

/// <summary>
/// Провайдер данных для отчета диаграммы Ганта
/// </summary>
public interface IRFCGanttQuery
{
    /// <summary>
    /// Сформировать отчет
    /// </summary>
    /// <param name="firstDay">Начальная дата</param>
    /// <param name="secondDay">Конечная дата</param>
    /// <returns></returns>
    Task<RFCGanttResultItem[]> ExecuteAsync(DateTime firstDay, DateTime secondDay, CancellationToken cancellationToken = default);
}