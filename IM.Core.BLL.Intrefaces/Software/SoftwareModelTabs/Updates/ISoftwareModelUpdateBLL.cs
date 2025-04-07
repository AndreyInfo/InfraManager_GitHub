using System.Threading.Tasks;
using System.Threading;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Updates;
public interface ISoftwareModelUpdateBLL
{
    /// <summary>
    /// Получение списка обновлений для конкретной модели ПО
    /// </summary>
    /// <param name="filter">Фильтр для вкладки модели ПО<see cref="SoftwareModelTabFilter"/></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных обновлений ПО.</returns>
    public Task<SoftwareModelUpdateListItemDetails[]> GetUpdatesForSoftwareModelAsync(SoftwareModelTabFilter filter, CancellationToken cancellationToken);
}
