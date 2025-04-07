using InfraManager.DAL.Software.Licenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software;
public interface ISoftwareModelQuery
{
    /// <summary>
    /// Получение списка моделей ПО.
    /// </summary>
    /// <param name="filter">Фильтр для поиска и пагинации.</param>
    /// <param name="orderedQuery">Query с сортировкой.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных моделей ПО.</returns>
    public Task<SoftwareModelDetailsItem[]> ExecuteAsync(PaggingFilter filter, IOrderedQueryable<SoftwareModel> orderedQuery, CancellationToken cancellationToken);
}
