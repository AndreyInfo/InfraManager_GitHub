using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using InfraManager.DAL.ServiceCatalogue.SLA;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

public interface IOperationalLevelAgreementBLL
{
    /// <summary>
    /// Добавляет новый OLA
    /// </summary>
    /// <param name="data">Данные для создания OLA</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<OperationalLevelAgreementDetails> AddAsync(OperationalLevelAgreementData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение OLA 
    /// </summary>
    /// <param name="id">ID OLA</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<OperationalLevelAgreementDetails> GetAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновляет OLA
    /// </summary>
    /// <param name="id">ID OLA, которое нужно обновить</param>
    /// <param name="data">Данные для обновления OLA</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<OperationalLevelAgreementDetails> UpdateAsync(int id, OperationalLevelAgreementData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет OLA
    /// </summary>
    /// <param name="id">ID OLA</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение списка OLA с фильтрацией
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<OperationalLevelAgreementDetails[]> ListAsync(BaseFilter filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все объекты, с кем заключено OLA
    /// </summary>
    /// <param name="id">ID OLA</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<SLAConcludedWithItem[]> GetConcludedWithAsync(Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить связь с Сервисом у данного OLA
    /// </summary>
    /// <param name="id">ID OLA, у которого нужно удалить сервис</param>
    /// <param name="serviceID">ID удаляемого сервиса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task RemoveServiceReferenceAsync(int id, Guid serviceID, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить связь с Сервисом у данного OLA
    /// </summary>
    /// <param name="id">ID OLA, к которому нужно добавить сервис</param>
    /// <param name="serviceID">ID добавляемого сервиса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task AddServiceReferenceAsync(int id, Guid serviceID, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все сервисы, которые связанны с данным OLA с фильтрацией
    /// </summary>
    /// <param name="id">ID OLA</param>
    /// <param name="filter">Фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<OperationalLevelAgreementServiceDetails[]> GetServiceReferenceAsync(int id, BaseFilter filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Создание OLA по аналогии
    /// </summary>
    /// <param name="id">ID OLA который нужно создать по аналогии</param>
    /// <param name="data">Дополнительные данные для создания OLA по аналогии</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task CloneAsync(int id, OperationalLevelAgreementCloneData data,
        CancellationToken cancellationToken = default);
}