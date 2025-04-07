using System.Threading.Tasks;
using System.Threading;
using InfraManager.DAL.Software;
using InfraManager.DAL;
using System;
using InfraManager.BLL.Software.SoftwareModelTabs.Dependencies;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Relations;
public interface ISoftwareModelRelationBLL
{
    /// <summary>
    /// Получение списка связанных моделей для конкретной модели ПО.
    /// </summary>
    /// <param name="filter">Фильтр для вкладки с фильтрацией по типу связи модели ПО<see cref="SoftwareModelTabDependencyFilter"/></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных связанных моделей ПО.</returns>
    Task<SoftwareModelRelatedListItemDetails[]> GetRelationsForSoftwareModelAsync(SoftwareModelTabDependencyFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Запрос получения связанных/доступных для связи моделей ПО.
    /// </summary>
    /// <param name="softwareModelID">Идентификатор модели ПО.</param>
    /// <param name="type">Тип связи.</param>
    /// <param name="isRelated">Флаг, указывающий на связанность(true)/доступность для связи(false).</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Запрос <see cref="IExecutableQuery"/> связанных/доступных для связи моделей ПО.</returns>
    Task<IExecutableQuery<SoftwareModel>> GetRelatedSoftwareModelsByIDAndTypeQueryAsync(
        Guid? softwareModelID,
        SoftwareModelDependencyType type,
        bool isRelated = true,
        CancellationToken cancellationToken = default);
}
