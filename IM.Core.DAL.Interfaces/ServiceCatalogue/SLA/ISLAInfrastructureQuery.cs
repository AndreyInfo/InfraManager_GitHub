using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.DAL.ServiceCatalogue.SLA;

public interface ISLAInfrastructureQuery
{
    ///  <summary>
    ///  Получение инфраструктуру данного SLA и Сервиса
    ///  </summary>
    ///  <param name="slaID">Идентификатор SLA</param>
    ///  <param name="searchString">Строка для поиска</param>
    ///  <param name="cancellationToken">токен отмены</param>
    ///  <param name="serviceID">Идентификатор Сервиса</param>
    ///  <param name="skip">пагинация, сколько объектов брать за раз</param>
    ///  <param name="take">пагинация, сколько объектов пропустить</param>
    ///  <returns>Инфраструктура SLA и Сервиса</returns>
    Task<PortfolioServiceInfrastructureItem[]> ExecuteAsync(Guid slaID, Guid serviceID, int? skip = null,
        int? take = null, string searchString = null, CancellationToken cancellationToken = default);
}