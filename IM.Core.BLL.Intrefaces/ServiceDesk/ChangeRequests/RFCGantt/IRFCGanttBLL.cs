using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests.RFCGantt;

/// <summary>
/// Работа с диаграммой Ганта для RFC
/// </summary>
public interface IRFCGanttBLL
{
    /// <summary>
    /// Достать/сформировать данные для диаграммы
    /// </summary>
    Task<IEnumerable<RFCGanttDetails>> GetAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Достать значение из сеттингов
    /// </summary>
    Task<int> GetViewSizeAsync(CancellationToken cancellationToken = default);
}