using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareModelTabs.PackageContents;
public interface ISoftwareModelPackageContentsBLL
{
    /// <summary>
    /// Получение списка моделей, входящих в конкретную модель пакета ПО.
    /// </summary>
    /// <param name="filter">Фильтр для вкладки модели ПО<see cref="SoftwareModelTabFilter"/></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных состава пакета.</returns>
    Task<SoftwareModelPackageContentsListItemDetails[]> GetPackageContentsForSoftwareModelAsync(SoftwareModelTabFilter filter, CancellationToken cancellationToken = default);
}
