using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace InfraManager.DAL.Software.Licenses;
public interface ISoftwareModelLicenseQuery
{
    /// <summary>
    /// Получение списка лицензий для конкретной модели ПО.
    /// </summary>
    /// <param name="filter">Фильтр для поиска и пагинации.</param>
    /// <param name="orderedQuery">Query с сортировкой.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных лицензий ПО.</returns>
    public Task<SoftwareModelLicenseItem[]> ExecuteAsync(PaggingFilter filter, IOrderedQueryable<SoftwareLicence> orderedQuery, CancellationToken cancellationToken);
}
