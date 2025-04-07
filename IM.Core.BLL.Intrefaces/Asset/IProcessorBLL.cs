using InfraManager.CrossPlatform.WebApi.Contracts.Assets;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Assets
{
    /// <summary>
    /// проведйр бизнес-логики Активов
    /// </summary>
    public interface IProcessorBLL
    {
        /// <summary>
        /// Получение списка моделей процессоров
        /// </summary>
        /// <param name="filter">  фильтер </param>        
        /// <param name="cancellationToken"> токен отмены </param>
        /// <returns> список моделей </returns>
        Task<BaseResult<List<ProcessorModelModel>, BaseError>> GetProcessorsListAsync(ProcessorsListFilter filter, CancellationToken cancellationToken);
    }
}
