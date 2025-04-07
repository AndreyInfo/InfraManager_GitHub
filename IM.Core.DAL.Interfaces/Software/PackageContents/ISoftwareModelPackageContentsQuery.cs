using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software.PackageContents;
public interface ISoftwareModelPackageContentsQuery
{
    /// <summary>
    /// Получение списка моделей, входящих в конкретную модель пакета ПО.
    /// </summary>
    /// <param name="filter">Фильтр для поиска и пагинации.</param>
    /// <param name="orderedQuery">Query с сортировкой.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных состава пакета.</returns>
    public Task<SoftwareModelPackageContentsItem[]> ExecuteAsync(PaggingFilter filter, IOrderedQueryable<SoftwareModel> orderedQuery, CancellationToken cancellationToken);
}
