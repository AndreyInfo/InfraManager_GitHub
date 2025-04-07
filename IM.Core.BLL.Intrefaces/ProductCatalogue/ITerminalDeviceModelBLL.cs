using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue;

/// <summary>
/// Бизнес логика работы с Моделями Терминальных Устройств (<see cref="ObjectClass.TerminalDeviceModel"/>)
/// </summary>
public interface ITerminalDeviceModelBLL
{
    /// <summary>
    /// Получить список элементов Модель Терминального Устройства, удовлетворяющих заданному фильтру, асинхронно.
    /// </summary>
    /// <param name="filterBy">Фильтр списка.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список элементов, удовлетворяющих заданному фильтру.</returns>
    Task<ProductModelDetails[]> GetDetailsArrayAsync(ProductModelListFilter filterBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить Модель Терминального Устройства по заданному идентификатору асихронно.
    /// </summary>
    /// <param name="id">Идентификатор объекта.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Экземпляр <see cref="ProductModelDetails"/>.</returns>
    Task<ProductModelDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
}