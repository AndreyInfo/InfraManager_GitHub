using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Asset.Adapters;
public interface IAdapterQuery
{
    /// <summary>
    /// Получение списка адаптеров.
    /// </summary>
    /// <param name="filter">Фильтр для поиска и пагинации.</param>
    /// <param name="orderedQuery">Query с сортировкой.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных адаптеров.</returns>
    public Task<AdapterItem[]> ExecuteAsync(PaggingFilter filter, IOrderedQueryable<Adapter> orderedQuery, CancellationToken cancellationToken);
}
