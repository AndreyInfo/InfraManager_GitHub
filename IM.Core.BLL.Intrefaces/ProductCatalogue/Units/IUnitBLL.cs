using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.Units;

/// <summary>
/// Бизнес-логика для работы с единицами измерений.
/// </summary>
public interface IUnitBLL
{
    /// <summary>
    /// Получение таблицы с фильтрацией, пагинацией, поиском и сортировкой.
    /// </summary>
    /// <param name="filter">Базовый фильтр <see cref="BaseFilter"/>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Массив данных типа <see cref="UnitDetails"/>.</returns>
    Task<UnitDetails[]> GetListAsync(BaseFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных единицы измерения.
    /// </summary>
    /// <param name="id">Идентификатор единицы измерения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные единицы измерения.</returns>
    Task<UnitDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление единицы измерения.
    /// </summary>
    /// <param name="data">Данные единицы измерения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные новой единицы измерения.</returns>
    Task<UnitDetails> AddAsync(UnitData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление единицы измерения ссответствующей идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор единицы измерения.</param>
    /// <param name="data">Новые данные для единицы измерения с тем же идентификатором.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные обновленной единицы измерения.</returns>
    Task<UnitDetails> UpdateAsync(Guid id, UnitData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление единицы измерения.
    /// </summary>
    /// <param name="id">Идентификатор единицы измерения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}