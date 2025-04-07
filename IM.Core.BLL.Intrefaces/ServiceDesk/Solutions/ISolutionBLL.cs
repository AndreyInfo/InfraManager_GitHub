using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Solutions;

public interface ISolutionBLL
{
    /// <summary>
    /// Добавление решения
    /// </summary>
    /// <param name="data">данные добавлемого решения</param>
    /// <param name="cancellationToken"></param>
    /// <returns>добавленная модель</returns>
    Task<SolutionDetails> AddAsync(SolutionData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление решения
    /// </summary>
    /// <param name="id">идентификатор обновляемой моедли</param>
    /// <param name="data">данные обновления</param>
    /// <param name="cancellationToken"></param>
    /// <returns>обновленная модель</returns>
    Task<SolutionDetails> UpdateAsync(Guid id, SolutionData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление решения
    /// </summary>
    /// <param name="id">идентификатор удаляемой модели</param>
    /// <param name="cancellationToken"></param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение решения по идентификатору
    /// </summary>
    /// <param name="id">идентификатор получаемого решения</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модель решения</returns>
    Task<SolutionDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение списка решений
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>массив моделей решений</returns>
    Task<SolutionDetails[]> GetAllDetailsArrayAsync(CancellationToken cancellationToken = default);
}
