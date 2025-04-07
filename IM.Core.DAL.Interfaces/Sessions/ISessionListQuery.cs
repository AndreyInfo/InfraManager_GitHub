using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Sessions;

/// <summary>
/// Выполняет запрос к базе данных и возвращает список сессий пользователей
/// </summary>
public interface ISessionListQuery
{
    /// <summary>
    /// Возвращает список сессий с полным путем до подразделения
    /// </summary>
    /// <param name="orderedQuery">Запрос, в котором настроена сортировка по нужной колонке</param>
    /// <param name="take">Сколько записей взять</param>
    /// <param name="skip">Сколько записей пропустить</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<SessionDetailsListItem[]> ExecuteAsync(IOrderedQueryable<Session> orderedQuery, int take,
        int skip, CancellationToken cancellationToken = default);
}