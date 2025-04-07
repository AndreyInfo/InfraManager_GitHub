using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;

public interface IServiceItemBLL
{
    /// <summary>
    /// Получать детальную информациб об элементе/услуге
    /// </summary>
    /// <param name="id">ItemID/AttendanceID</param>
    Task<ServiceItemDetailsModel> DetailsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить детальную информацию об элементе/услуге по ID из таблицы CallSummary
    /// </summary>
    /// <param name="id">CallSummaryID</param>
    Task<ServiceItemDetailsModel> DetailsByCallSummaryIDAsync(Guid id, CancellationToken cancellationToken = default);
}
