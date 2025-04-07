using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Asset;

namespace InfraManager.BLL.ServiceDesk.ServiceUnits
{
    /// <summary>
    /// Логика работы с исполнителями сервисных блоков
    /// </summary>
    public interface IServiceUnitPerformersBLL
    {
        /// <summary>
        /// получение списка исполнителей сервисного блока, по id Сервисного блока
        /// </summary>
        /// <param name="serviceUnitId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PerformerDetails[]> GetPerformersByServiceUnitIdAsync(Guid serviceUnitId, CancellationToken cancellationToken);

        /// <summary>
        /// добавление исполнителя к сервисному блоку
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddPerformersAsync(PerformerServiceUnitDetails model, CancellationToken cancellationToken);

        /// <summary>
        /// удаление исполнителя у сервисного блока
        /// </summary>
        /// <param name="models"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeletePerformersAsync(PerformerServiceUnitDetails[] models, CancellationToken cancellationToken);
    }
}
