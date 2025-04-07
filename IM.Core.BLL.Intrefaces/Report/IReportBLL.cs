using InfraManager.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Report
{
    /// <summary>
    /// Интерфейс для бизнес-логики Отчетов
    /// </summary>
    public interface IReportBLL
    {
        /// <summary>
        /// Получение списка отчетов
        /// </summary>
        /// <param name="filter">Фильтр сортировки</param>
        /// <param name="cancellationToken"> токен отмены </param>
        /// <returns> список отчетов </returns>
        Task<ReportForTableDetails[]> GetReportsAsync(ReportsFilter filter, CancellationToken cancellationToken);
        /// <summary>
        /// Получение списка отчетов
        /// </summary>
        /// <param name="cancellationToken"> токен отмены </param>
        /// <returns> список отчетов </returns>
        Task<ReportDetails[]> GetReportsAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Обновление шаблона отчета
        /// </summary>
        /// <param name="id"> идентификатор отчета </param>
        /// <param name="data"> шаблон отчета </param>
        /// <param name="cancellationToken"> токен отмены </param>
        /// <returns> список отчетов </returns>
        Task UpdateReportDataAsync(Guid id, string data, CancellationToken cancellationToken = default);
        /// <summary>
        /// Получение отчета по идентификатору
        /// </summary>
        /// <param name="id"> идентификатор отчета</param>
        /// <param name="cancellationToken"> токен отмены </param>
        /// <returns> отчет </returns>
        Task<ReportDetails> GetReportAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Удаление отчета из базы данных
        /// </summary>
        /// <param name="id"> идентификатор отчета</param>
        /// <param name="cancellationToken"> токен отмены </param>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Добавление отчета в базу данных
        /// </summary>
        /// <param name="report"> отчет</param>
        /// <param name="cancellationToken"> токен отмены </param>
        Task InsertAsync(ReportData report, CancellationToken cancellationToken);

        /// <summary>
        /// Обновление отчета в базе данных
        /// </summary>
        /// <param name="id">ID отчета</param>
        /// <param name="report"> отчет</param>
        /// <param name="cancellationToken"> токен отмены </param>
        Task UpdateAsync(Guid id, ReportData report, CancellationToken cancellationToken);
    }
}
