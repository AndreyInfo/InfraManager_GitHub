using InfraManager.BLL.Software.SoftwareModels;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Dependencies;
public interface ISoftwareModelDependencyBLL
{
    /// <summary>
    /// Получение списка доступных зависимостей для конкретной модели ПО.
    /// </summary>
    /// <param name="filter">Фильтр для вкладки с фильтрацией по типу связи модели ПО<see cref="SoftwareModelTabDependencyFilter"/></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных досутпных моделей каталога ПО типа <see cref="SoftwareModelListItemDetails"/>.</returns>
    Task<SoftwareModelListItemDetails[]> GetDependenciesForSoftwareModelAsync(SoftwareModelTabDependencyFilter filter, CancellationToken cancellationToken = default);
}
