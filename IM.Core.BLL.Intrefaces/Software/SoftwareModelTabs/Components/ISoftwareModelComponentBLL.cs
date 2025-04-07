using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Components;
public interface ISoftwareModelComponentBLL
{
    /// <summary>
    /// Получение списка компонентов для конкретной модели ПО.
    /// </summary>
    /// <param name="filter">Фильтр для вкладки модели ПО<see cref="SoftwareModelTabFilter"/></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных компонент ПО.</returns>
    Task<SoftwareModelComponentListItemDetails[]> GetComponentsForSoftwareModelAsync(SoftwareModelTabFilter filter, CancellationToken cancellationToken = default);
}
