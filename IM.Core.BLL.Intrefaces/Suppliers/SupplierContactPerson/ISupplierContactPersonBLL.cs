using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Suppliers.SupplierContactPerson;

/// <summary>
/// Бизнес-логика для работы с Контактными лицами.
/// </summary>
public interface ISupplierContactPersonBLL
{
    /// <summary>
    /// Получение таблицы с фильтрацией, пагинацией, поиском и сортировкой.
    /// </summary>
    /// <param name="filter">Фильтр типа <see cref="SupplierContactPersonFilter"/>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Массив данных типа <see cref="SupplierContactPersonDetails"/>.</returns>
    Task<SupplierContactPersonDetails[]> GetListAsync(SupplierContactPersonFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Получение Контактного лица
    /// </summary>
    /// <param name="id">Идентификатор Контактного лица</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Данные Контактного лица <see cref="SupplierContactPersonDetails"/>.</returns>
    Task<SupplierContactPersonDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Создание нового Контактного лица.
    /// </summary>
    /// <param name="data">Данные Контактного лица.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные нового Контактного лица <see cref="SupplierContactPersonDetails"/>.</returns>
    Task<SupplierContactPersonDetails> AddAsync(SupplierContactPersonData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление Контактного лица.
    /// </summary>
    /// <param name="id">Идентификатор Контактного лица.</param>
    /// <param name="data">Данные Контактного лица.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные обновленного Контактного лица <see cref="SupplierContactPersonDetails"/>.</returns>
    Task<SupplierContactPersonDetails> UpdateAsync(Guid id, SupplierContactPersonData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление Контактного лица.
    /// </summary>
    /// <param name="id">Идентификатор Контактного лица.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}