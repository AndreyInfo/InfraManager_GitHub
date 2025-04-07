using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceCatalogue.SLA;

public interface ISLAConcludedWithQuery
{
    /// <summary>
    /// Получение списка объектов, с кем заключенно данное SLA
    /// </summary>
    /// <param name="slaID">Идентификатор SLA</param>
    /// <param name="cancellationToken">токен отмены</param>
    ///<returns>Список объектов, с кем заключенно SLA</returns>
    Task<SLAConcludedWithItem[]> ExecuteAsync(Guid slaID, CancellationToken cancellationToken = default);
}