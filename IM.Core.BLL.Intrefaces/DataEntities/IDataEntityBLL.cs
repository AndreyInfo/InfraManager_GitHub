using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.Calls.DTO;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Threading;
using InfraManager.BLL.DataEntities.DTO;

namespace InfraManager.BLL.DataEntities
{
    /// <summary>
    /// Служба DataEntity
    /// </summary>
    public interface IDataEntityBLL
    {
        /// <summary>
        /// Получить DataEntity
        /// </summary>
        /// <param name="id">Идентификатор DataEntity</param>
        /// <param name="cancellationToken">Ключ отмены</param>
        /// <returns>DataEntityDetails</returns>
        public Task<DataEntityDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);
    }
}
