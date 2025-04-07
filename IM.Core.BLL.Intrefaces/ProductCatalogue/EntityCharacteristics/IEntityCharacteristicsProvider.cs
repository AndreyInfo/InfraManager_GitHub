using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ModelCharacteristics;

/// <summary>
/// Интерфейс провайдера для характеристик модели/объекта.
/// </summary>
public interface IEntityCharacteristicsProvider
{
    /// <summary>
    /// Получение данных конкретных характеристик модели/объекта в зависимости от подкласса.
    /// </summary>
    /// <param name="id">Идентификатор модели/объекта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные конкретных характеристик модели/объекта.</returns>
    Task<EntityCharacteristicsDetailsBase> GetAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление конкретных характеристик модели/объекта в зависимости от подкласса.
    /// </summary>
    /// <param name="data">Данные характеристик модели/объекта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task InsertAsync(EntityCharacteristicsDataBase data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление конкретных характеристик модели/объекта в зависимости от подкласса
    /// </summary>
    /// <param name="id">Идентификатор модели/объекта.</param>
    /// <param name="data">Данные характеристик модели/объекта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные обновленных характеристик модели/объекта.</returns>
    Task<EntityCharacteristicsDetailsBase> UpdateAsync(Guid id, EntityCharacteristicsDataBase data, CancellationToken cancellationToken);

}
