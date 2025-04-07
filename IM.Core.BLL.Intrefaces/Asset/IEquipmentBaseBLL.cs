using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset;

/// <summary>
/// Бизнес логика для работы с оборудованием.
/// </summary>
/// <typeparam name="TKey">Тип ключа.</typeparam>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
/// <typeparam name="TData">Тип контракта входных данных.</typeparam>
/// <typeparam name="TDetails">Тип контракта выходных данных.</typeparam>
public interface IEquipmentBaseBLL<TKey, TEntity, TData, TDetails>
{
    /// <summary>
    /// Получить оборудование по ключу.
    /// </summary>
    /// <param name="id">Идентификатор оборудования.</param>
    /// <param name="cancellationToken">Ключ отмены.</param>
    /// <returns>Детали оборудования.</returns>
    Task<TDetails> DetailsAsync(TKey id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление оборудования.
    /// </summary>
    /// <param name="data">Данные оборудования.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные нового оборудования.</returns>
    Task<TDetails> AddAsync(TData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление оборудования.
    /// </summary>
    /// <param name="id">Идентификатор оборудования.</param>
    /// <param name="data">Данные оборудования.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные обновленного оборудования.</returns>
    Task<TDetails> UpdateAsync(TKey id, TData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление оборудования.
    /// </summary>
    /// <param name="id">Идентификатор оборудования.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task DeleteAsync(TKey id, CancellationToken cancellationToken);
}
