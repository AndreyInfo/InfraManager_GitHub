using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Sessions;

/// <summary>
/// Реализует выполнение запроса к базе данных для получения списка активных персональных сессий
/// </summary>
public interface IActivePersonalSessionCountQuery
{
    /// <summary>
    /// Выполняет запрос к базе данных и возвращает список активных сессий
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<int> ExecuteAsync(CancellationToken cancellationToken = default);
}