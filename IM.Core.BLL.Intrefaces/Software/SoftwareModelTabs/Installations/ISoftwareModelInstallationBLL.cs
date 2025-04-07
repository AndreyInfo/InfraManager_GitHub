using System.Threading.Tasks;
using System.Threading;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Installations;
public interface ISoftwareModelInstallationBLL
{
    /// <summary>
    /// Получение списка инсталляций для конкретной модели ПО.
    /// </summary>
    /// <param name="filter">Фильтр для вкладки модели ПО<see cref="SoftwareModelTabFilter"/></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных инсталяций ПО.</returns>
    Task<SoftwareModelInstallationListItemDetails[]> GetInstallationsForSoftwareModelAsync(SoftwareModelTabFilter filter, CancellationToken cancellationToken = default);
}
