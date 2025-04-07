using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public interface ICallSummaryBLL
    {
        /// <summary>
        /// Список описаний заявок
        /// </summary>
        /// <param name="serviceId">Идентификатор</param>
        /// <param name="classId">Идентификатор сущности</param>
        /// <param name="filter">Базовый фильтр</param>
        /// <returns></returns>
        public Task<CallSummaryDetails[]> GetListAsync(CallSummaryFilter filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавляет или обновляет описание заявки
        /// </summary>
        /// <param name="callSummary">Заявка</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        public Task<Guid> AddOrUpdateAsync(CallSummaryDetails callSummary,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// Удаление заявки
        /// </summary>
        /// <param name="deleteModels">Список идентификаторов описаний заявок на удаление</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string[]> DeleteAsync(List<DeleteModel<Guid>> deleteModels,
            CancellationToken cancellationToken = default);
    }
}
