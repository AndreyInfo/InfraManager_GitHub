using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Problems;

/// <summary>
/// Работы с зависимостями проблемы.
/// </summary>
public interface IProblemDependenciesBLL
{
    /// <summary>
    /// Получить список зависимостей проблемы, которые удовлетворяют фильтру, асинхронно. 
    /// </summary>
    /// <param name="filterBy">Фильтр.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns></returns>
    Task<ProblemDependencyQueryResultItem[]> GetDetailsArrayAsync(ProblemDependencyByProblemFilter filterBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить зависимость проблемы асинхронно.
    /// </summary>
    /// <param name="data">Данные для создания зависимости.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns></returns>
    Task<ProblemDependencyQueryResultItem> AddAsync(ProblemDependencyDetailsModel data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить зависимость проблемы с заданным идентификатором асинхронно.
    /// </summary>
    /// <param name="id">Уникальный идентификатор объекта.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns></returns>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}