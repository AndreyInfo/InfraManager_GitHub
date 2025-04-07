using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Search
{
    /// <summary>
    /// Стратегия поиска объектов Service Desk с использованием параметров 
    /// </summary>
    /// <typeparam name="T">Тип параметров необходимых для поиска</typeparam>
    public interface IServiceDeskSearchStrategy<in T> where T : ServiceDeskSearchParameters
    {
        /// <summary>
        /// Совершает поиск объектов
        /// </summary>
        /// <param name="searchParameters">Параметры для поиска</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Найденные объекты</returns>
        Task<IReadOnlyList<FoundObject>> SearchAsync(T searchParameters, CancellationToken cancellationToken = default);
    }
}