using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.PortAdapter;

/// <summary>
/// Бизнес-логика для работы с портами для объекта адаптер.
/// </summary>
public interface IPortAdapterBLL
{
    /// <summary>
    /// Получение таблицы с фильтрацией, пагинацией, поиском и сортировкой.
    /// </summary>
    /// <param name="filter">Фильтр типа <see cref="PortAdapterFilter"/>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Массив данных типа <see cref="PortAdapterDetails"/>.</returns>
    Task<PortAdapterDetails[]> GetListAsync(PortAdapterFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Получение порта.
    /// </summary>
    /// <param name="id">Идентификатор порта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные порта <see cref="PortAdapterDetails"/>.</returns>
    Task<PortAdapterDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Создание нового порта.
    /// </summary>
    /// <param name="data">Данные порта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные нового порта типа <see cref="PortAdapterDetails"/>.</returns>
    Task<PortAdapterDetails> AddAsync(PortAdapterData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление порта.
    /// </summary>
    /// <param name="id">Идентификатор порта.</param>
    /// <param name="data">Данные порта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные обновленного порта типа <see cref="PortAdapterDetails"/>.</returns>
    Task<PortAdapterDetails> UpdateAsync(Guid id, PortAdapterData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление порта.
    /// </summary>
    /// <param name="id">Идентификатор порта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}

