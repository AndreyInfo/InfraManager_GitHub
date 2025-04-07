using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

/// <summary>
/// Внутренний интерфейся, который работает с объектами имеющими в себе теги и линии поддержки
/// конкретно делался для Service, ServiceItem, ServiceAttendence
/// </summary>
/// <typeparam name="TDetails">модель для получения</typeparam>
/// <typeparam name="TData">модель для сохранение</typeparam>
public interface IPortfolioServiceItemBLL<TDetails, TData>
    where TDetails : PortfolioServcieItemDetails
    where TData : PortfolioServcieItemData
{
    /// <summary>
    /// Сохранение линии поддержки и тегов
    /// Если наследуется от сервиса, то копирование линий поддержки из сервиса к объекту поддерживается
    /// </summary>
    /// <param name="id">идентификатор сущности для которой сохроняют теги и линии поддержки</param>
    /// <param name="intputModel">модель с данными</param>
    /// <param name="cancellationToken"></param>
    Task SaveSupportLinAndTagsAsync(Guid id, TData intputModel, CancellationToken cancellationToken);

    /// <summary>
    /// Инициалиация линий поддержки и тегов у объекта
    /// </summary>
    /// <param name="details"></param>
    /// <param name="cancellationToken"></param>
    Task InitializateSupportLineAndTagsAsync(TDetails details, CancellationToken cancellationToken);
}