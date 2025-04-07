using InfraManager.BLL.Asset;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ReportsForCommand;

public interface IReportsForCommandBLL
{
    /// <summary>
    /// Получить объект команда-отчёт
    /// </summary>
    /// <param name="id">Идентификатор команды</param>
    /// <returns>Объект команда-отчёт</returns>
    Task<ReportForCommandDetails> GetAsync(byte id, CancellationToken cancellationToken);

    /// <summary>
    /// Получить массив всех объектов отчёт-команда
    /// </summary>
    /// <returns>Массив объектов отчёт-команда</returns>
    Task<ReportForCommandDetails[]> GetListAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Добавить связь отчёт-команда
    /// </summary>
    /// <param name="data">Данные для добавления</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Объект отчёт-команда</returns>
    Task<ReportForCommandDetails> AddAsync(ReportForCommandData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновить связь отчёт-команда
    /// </summary>
    /// <param name="id">Идентификатор команды</param>
    /// <param name="data">Данные для обновления</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Объект отчёт-команда</returns>
    Task<ReportForCommandDetails> UpdateAsync(byte id, ReportForCommandData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удалить связь отчёт-команда
    /// </summary>
    /// <param name="id">Идентификатор команды</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}