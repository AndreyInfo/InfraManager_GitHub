using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Sessions;

/// <summary>
/// Реализует выполнение запроса к базе данных для получения списка конкурентных сессий
/// </summary>
public interface IActiveEngineerSessionCountQuery
{
     /// <summary>
     /// Выполняет запрос к базе данных и возвращает список конкурентных сессий
     /// </summary>
     /// <param name="cancellationToken">Токен отмены</param>
     Task<int> ExecuteAsync(CancellationToken cancellationToken = default);
}