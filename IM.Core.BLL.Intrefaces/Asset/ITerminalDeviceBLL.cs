using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset
{
    /// <summary>
    /// Служба оконечного оборудования
    /// </summary>
    public interface ITerminalDeviceBLL
    {
        /// <summary>
        /// Получить Оконечное оборудование
        /// </summary>
        /// <param name="id">Идентификатор Оконечного оборудования</param>
        /// <param name="cancellationToken">Ключ отмены</param>
        /// <returns>Детали Оконечного оборудования</returns>
        Task<TerminalDeviceDetails> DetailsAsync(int id, CancellationToken cancellationToken = default);
    }
}
