using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public interface ICallTypeBLL
    {
        Task<CallTypeDetails[]> GetDetailsPageAsync(
            CallTypeListFilter filterBy, 
            CancellationToken cancellationToken = default);
        Task<CallTypeDetails> DetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавляет новый тип заявки
        /// </summary>
        /// <param name="data">Данные заявки</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Добавленный тип заявок SLA</returns>
        Task<CallTypeDetails> AddAsync(CallTypeData data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновляет тип заявки
        /// </summary>
        /// <param name="id">Идентификатор типа заявки</param>
        /// <param name="data">Данные SLA</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ID добавленного SLA</returns>
        Task<CallTypeDetails> UpdateAsync(
            Guid id, 
            CallTypeData data,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает иконку типа заявки в виде массива байтов
        /// </summary>
        /// <param name="callTypeID">Идентификатор типа заявки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив байтов иконки</returns>
        Task<byte[]> GetImageBytesAsync(
            Guid callTypeID, 
            CancellationToken cancellationToken = default);
    }
}
