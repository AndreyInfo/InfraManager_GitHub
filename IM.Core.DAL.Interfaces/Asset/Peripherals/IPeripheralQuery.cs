using InfraManager.DAL.Asset.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Asset.Peripherals;
public interface IPeripheralQuery
{
    /// <summary>
    /// Получение списка периферийных устройств.
    /// </summary>
    /// <param name="filter">Фильтр для поиска и пагинации.</param>
    /// <param name="orderedQuery">Query с сортировкой.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных периферийных устройств.</returns>
    public Task<PeripheralItem[]> ExecuteAsync(PaggingFilter filter, IOrderedQueryable<Peripheral> orderedQuery, CancellationToken cancellationToken);
}
