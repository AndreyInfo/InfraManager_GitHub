using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk;

public interface ISetAgreement<TEntity>
{
    /// <summary>
    /// Вычисляет подходящий SLA/OLA для сущности и в зависимости от реализации заполняет доп. свойства
    /// </summary>
    /// <param name="entity">Ссылка на сущность</param>
    /// <param name="agreementID">"ID предварительно заданного SLA/OLA"</param>
    /// <param name="countCompleteDateFrom">Время, от которого вычисляются обещанная дата и время закрытия</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SetAsync(TEntity entity, CancellationToken cancellationToken = default, DateTime? countCompleteDateFrom = null, Guid? agreementID = null);
}