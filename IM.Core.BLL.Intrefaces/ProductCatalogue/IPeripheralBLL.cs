using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue
{
    /// <summary>
    /// Служба Периферии
    /// </summary>
    public interface IPeripheralBLL
    {
        /// <summary>
        /// Получить Периферию
        /// </summary>
        /// <param name="id">Идентификатор Периферии</param>
        /// <param name="cancellationToken">Ключ отмены</param>
        /// <returns>Детали Периферии</returns>
        Task<PeripheralDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
