using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;

/// <summary>
/// Бизнес логика работы с Сервисами
/// </summary>
public interface IServiceBLL
{
    /// <summary>
    /// Получение списка сервисов
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ServiceDetails[]> GetListByCategoryIDAsync(Guid categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение таблицы с возможностью скронлинга и поиска для таблицы
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="id"></param>
    /// <param name="classID"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PortfolioServiceItemTable[]> GetServicesForTableAsync(ServiceFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение сервиса по id, с линиями поддержки и тегами
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ServiceDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление, теги и линии поддержки перезаписываются тоже
    /// </summary>
    /// <param name="model"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> UpdateAsync(ServiceData model, Guid id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Добавление сервиса с линиями поддержки и тегами
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> AddAsync(ServiceData model, CancellationToken cancellationToken = default);

    Task<ServiceDetailsModel> FindAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
