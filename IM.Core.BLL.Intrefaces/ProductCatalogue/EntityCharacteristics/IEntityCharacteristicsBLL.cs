using InfraManager.DAL.ProductCatalogue;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ModelCharacteristics;

/// <summary>
/// Бизнес-логика для работы с характеристиками модели/объекта.
/// </summary>
public interface IEntityCharacteristicsBLL
{
    /// <summary>
    /// Получение характеристик модели/объекта.
    /// </summary>
    /// <param name="id">Идентификатор модели/объекта.</param>
    /// <param name="templateID">Идентификатор класса.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Характеристики модели/объекта.</returns>
    public Task<EntityCharacteristicsDetailsBase> GetAsync(Guid id, ProductTemplate templateID, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление характеристик модели/объекта.
    /// </summary>
    /// <param name="id">Идентификатор модели/объекта.</param>
    /// <param name="templateID">Идентификатор класса.</param>
    /// <param name="data">Данные характеристик модели/объекта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Характеристики модели/объекта.</returns>
    public Task<EntityCharacteristicsDetailsBase> UpdateAsync(Guid id, ProductTemplate templateID, EntityCharacteristicsDataBase data, CancellationToken cancellationToken);
}
