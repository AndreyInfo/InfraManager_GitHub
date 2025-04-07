using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue;

public interface IServiceLevelAgreementReference
{
    /// <summary>
    /// Добавляет связь между SLA и объектом(для изменения временной зоны и графика работ)
    /// </summary>
    /// <param name="slaReferenceDetails">Данные SLA и объекта для связи</param>
    /// <param name="cancellationToken">токен отмены</param>
    Task InsertAsync(SLAReferenceDetails slaReferenceDetails, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет связь между SLA и объектом
    /// </summary>
    /// <param name="slaID">Идентификатор SLA</param>
    /// <param name="objectID"> Идентификатор Объекта</param>
    /// <param name="cancellationToken">токен отмены</param>
    Task DeleteAsync(Guid slaID, Guid objectID, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает связь между SLA и объектом
    /// </summary>
    /// <param name="slaID">Идентификатор SLA</param>
    /// <param name="objectID"> Идентификатор Объекта</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>Связанные данные между объектом и SLA</returns>
    Task<SLAReferenceDetails> GetAsync(Guid slaID, Guid objectID, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновляет значения связи между SLA и объектом
    /// </summary>
    /// <param name="slaID">Идентификатор SLA</param>
    /// <param name="objectID"> Идентификатор Объекта</param>
    /// <param name="slaReferenceDetails">Данные SLA и объекта для связи</param>
    /// <param name="cancellationToken">токен отмены</param>
    Task UpdateAsync(Guid slaID, Guid objectID, SLAReferenceData slaReferenceDetails,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает связи между SLA и объектам
    /// </summary>
    /// <param name="slaID">Идентификатор SLA</param>
    /// <param name="classID"> Идентификатор класса</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>Связанные данные между объектом и SLA</returns>
    Task<SLAReferenceDetails[]> GetListAsync(Guid slaID, ObjectClass classID, CancellationToken cancellationToken = default);
}