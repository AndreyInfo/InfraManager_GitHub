using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Search;

public interface ILightSearcher
{
    /// <summary>
    /// В зависимости от фильтра ищет заявки/задания/массовые инциденты/проблемы в системе
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<MyTasksReportItem[]> SearchAsync(SearchFilter filter, CancellationToken cancellationToken = default);
}