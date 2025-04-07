using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Licenses;
public interface ISoftwareModelLicenseBLL
{
    /// <summary>
    /// Получение списка лицензий для конкретной модели ПО.
    /// </summary>
    /// <param name="filter">Фильтр для вкладки модели ПО<see cref="SoftwareModelTabFilter"/></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных лицензий ПО.</returns>
    Task<SoftwareModelLicenseListItemDetails[]> GetLicensesForSoftwareModelAsync(SoftwareModelTabFilter filter, CancellationToken cancellationToken = default);
}
