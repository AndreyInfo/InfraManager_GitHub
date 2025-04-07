using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ServiceCatalogue.SLA;

namespace InfraManager.BLL.ServiceCatalogue
{
    public interface IServiceLevelAgreementBLL
    {

        /// <summary>
        /// Добавляет новое SLA
        /// </summary>
        /// <param name="data">Данные SLA</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ID добавленного SLA</returns>
        Task<Guid> InsertAsync(ServiceLevelAgreementData data,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Обновляет SLA
        /// </summary>
        /// <param name="id">id SLA</param>
        /// <param name="data">Данные для замены SLA</param>
        /// <param name="cancellationToken"></param>
        Task UpdateAsync(Guid id, ServiceLevelAgreementData data,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет SLA
        /// </summary>
        /// <param name="slaID">ID SLA для удаления</param>
        /// <param name="cancellationToken"></param>
        Task DeleteAsync(Guid slaID, CancellationToken cancellationToken = default);


        /// <summary>
        /// Получение списка SLA по заданному фильтру
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Список SLA</returns>
        Task<ServiceLevelAgreementDetails[]> ListAsync(SLAFilter filter,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Получение SLA по ID
        /// </summary>
        /// <param name="slaID">ID SLA, которое нужно получить</param>
        /// <param name="cancellationToken"></param>
        /// <returns>SLA</returns>
        Task<ServiceLevelAgreementDetails> GetAsync(Guid slaID,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Получение списка с кем было заключенно конкретное SLA
        /// </summary>
        /// <param name="slaID">ID SLA</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Список объектов, с кем заключено SLA</returns>
        Task<SLAConcludedWithItem[]> GetConcludedWithAsync(Guid slaID,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение списка OrganizationItemGroup
        /// </summary>
        /// <param name="slaID">ID SLA</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Список объектов OrganizationItemGroup</returns>
        Task<OrganizationItemGroupData[]> GetOrganizationItemGroupsAsync(Guid slaID,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение свободной инфраструктуры определенного SLA и Сервиса
        /// </summary>
        /// <param name="slaID">ID SLA</param>
        /// <param name="portfolioServiceID">ID портфеля сервиса</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Список свободной инфраструктуры</returns>
        Task<PortfolioServiceInfrastructureItem[]> FreeInfrastructureAsync(Guid portfolioServiceID,
            Guid slaID, CancellationToken cancellationToken = default);


        /// <summary>
        /// Получение инфраструктуры определенного SLA и Сервиса
        /// </summary>
        /// <param name="slaID">ID SLA</param>
        /// <param name="portfolioServiceID">ID портфеля сервиса</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Список свободной инфраструктуры</returns>
        Task<PortfolioServiceInfrastructureItem[]> InfrastructureAsync(Guid slaID, Guid serviceID, int? skip = null,
            int? take = null, string searchString = null, CancellationToken cancellationToken = default);


        /// <summary>
        /// Добавляет инфраструктуру к Сервису и SLA
        /// (объект для инфраструктуры можно добавить только
        /// такой, который добавлен в инфраструктуру Сервиса)
        /// </summary>
        /// <param name="slaID">ID SLA</param>
        /// <param name="serviceReferenceID">ID ServiceReference</param>
        /// <param name="cancellationToken"></param>
        Task InsertInfrastructureAsync(Guid slaID, Guid serviceReferenceID,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Удаляет инфраструктуру Сервиса и SLA
        /// </summary>
        /// <param name="slaID">ID SLA</param>
        /// <param name="serviceReferenceID">ID ServiceReference</param>
        /// <param name="cancellationToken"></param>
        Task DeleteInfrastructureAsync(Guid slaID, Guid serviceReferenceID,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Создает SLA по аналогии
        /// </summary>
        /// <param name="slaID">ID SLA, по аналогии которого нужно создать SLA</param>
        /// <param name="data">Информация нового SLA</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task CloneAsync(Guid slaID, ServiceLevelAgreementData data,
            CancellationToken cancellationToken = default);
    }
}
