using InfraManager.BLL.ProductCatalogue.SlotTemplates;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.Slots;

/// <summary>
/// Бизнес-логика для работы со слотами моделей/объектов.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
/// <typeparam name="TData">Тип контракта входных данных.</typeparam>
/// <typeparam name="TDetails">Тип контракта выходных данных.</typeparam>
/// <typeparam name="TTable">Тип таблицы.</typeparam>
public interface ISlotBaseBLL<TEntity, TData, TDetails, TTable>
{
    /// <summary>
    /// Получение таблицы с фильтрацией, пагинацией, поиском и сортировкой.
    /// </summary>
    /// <param name="filter">Фильтр типа <see cref="SlotBaseFilter"/>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Массив данных слотов.</returns>
    Task<TDetails[]> GetListAsync(SlotBaseFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Получение слота.
    /// </summary>
    /// <param name="key">Идентификатор слота.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные слота.</returns>
    Task<TDetails> DetailsAsync(SlotBaseKey key, CancellationToken cancellationToken);

    /// <summary>
    /// Создание нового слота.
    /// </summary>
    /// <param name="data">Данные слота.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные нового слота.</returns>
    Task<TDetails> AddAsync(TData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление слота.
    /// </summary>
    /// <param name="key">Идентификатор слота.</param>
    /// <param name="data">Данные слота.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные обновленного слота типа.</returns>
    Task<TDetails> UpdateAsync(SlotBaseKey key, TData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление слота.
    /// </summary>
    /// <param name="key">Идентификатор слота.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task DeleteAsync(SlotBaseKey key, CancellationToken cancellationToken);
}
