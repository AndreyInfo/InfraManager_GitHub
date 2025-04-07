using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue;

/// <summary>
/// Бизнес логика работы с Моделями Сетевых Устройств (<see cref="ObjectClass.NetworkDeviceModel"/>)
/// </summary>
public interface INetworkDeviceModelBLL
{
    /// <summary>
    /// Получить список элементов Модель Сетевого Устройства, удовлетворяющих заданному фильтру, асинхронно.
    /// </summary>
    /// <param name="filterBy">Фильтр списка.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список элементов, удовлетворяющих заданному фильтру.</returns>
    Task<ProductModelDetails[]> GetDetailsArrayAsync(ProductModelListFilter filterBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить Модель Сетевого Устройства по заданному идентификатору асихронно.
    /// </summary>
    /// <param name="id">Идентификатор объекта.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Экземпляр <see cref="NetworkDeviceModelDetails"/>.</returns>
    Task<ProductModelDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
}