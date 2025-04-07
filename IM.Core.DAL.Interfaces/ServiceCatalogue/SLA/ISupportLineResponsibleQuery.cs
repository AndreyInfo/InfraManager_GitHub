using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceCatalogue.SLA;

public interface ISupportLineResponsibleQuery
{
    /// <summary>
    /// Получает список ответственных по линии поддержки. Если для услуги список ответственных пуст, то берется список ответственных для родительского сервиса
    /// </summary>
    /// <param name="objectID">ID услуги</param>
    /// <param name="objectClassID">ID класса</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<SupportLineResponsible>>
        ExecuteAsync(Guid objectID, ObjectClass objectClassID, CancellationToken cancellationToken = default);
}