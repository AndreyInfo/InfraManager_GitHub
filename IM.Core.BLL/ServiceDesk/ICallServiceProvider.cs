using InfraManager.DAL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public interface ICallServiceProvider
    {
        /// <summary>
        /// Ищет подходящий объект агрегата CallService или создает новую
        /// </summary>
        /// <param name="serviceItemOrAttendanceID">Идентификатор услуги или элемента сервиса</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Ссылка на экземпляр объекта агрегата CallService</returns>
        Task<CallService> GetOrCreateAsync(Guid? serviceItemOrAttendanceID, CancellationToken cancellationToken = default);
    }
}
