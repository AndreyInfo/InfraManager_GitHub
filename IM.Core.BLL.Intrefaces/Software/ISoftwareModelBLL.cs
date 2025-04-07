using System.Threading.Tasks;
using System.Threading;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using InfraManager.BLL.Software.SoftwareModels;

namespace InfraManager.BLL.Software;

/// <summary>
/// Бизнес логика для работы с сущностью SoftwareModel
/// </summary>
public interface ISoftwareModelBLL
{
    /// <summary>
    /// Получение списка ПО
    /// </summary>
    /// <param name="filter">Базовый фильтр <see cref="BaseFilter"/></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных каталога ПО типа <see cref="SoftwareModelListItemDetails"/>.</returns>
    Task<SoftwareModelListItemDetails[]> GetListAsync(BaseFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение конкретной модели ПО по ID
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Одну из реализаций типа <see cref="SoftwareModelDetailsBase"/> в зависимости от шаблона модели ПО.</returns>
    Task<SoftwareModelDetailsBase> GetAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавление новой модели ПО
    /// </summary>
    /// <param name="data">Данные модели ПО</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Данные о новой модели ПО типа <see cref="SoftwareModelDetailsBase"/>.</returns>
    Task<SoftwareModelDetailsBase> AddAsync(SoftwareModelData data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохранение изменений конкрентной модели ПО
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="data">Данные модели ПО</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Данные обновленной модели ПО типа <see cref="SoftwareModelDetailsBase"/>.</returns>
    Task<SoftwareModelDetailsBase> UpdateAsync(Guid id, SoftwareModelData data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление конкрентной модели ПО
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
