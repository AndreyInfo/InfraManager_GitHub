using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.Calls.DTO;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Threading;
using InfraManager.BLL.DataEntities.DTO;
using InfraManager.BLL.ConfigurationUnit.DTO;

namespace InfraManager.BLL.ConfigurationUnit
{
    /// <summary>
    /// Служба ConfigurationUnit
    /// </summary>
    public interface IConfigurationUnitBLL
    {
        /// <summary>
        /// Получить ConfigurationUnit
        /// </summary>
        /// <param name="id">Идентификатор ConfigurationUnit</param>
        /// <param name="cancellationToken">Ключ отмены</param>
        /// <returns>ConfigurationUnitDetails</returns>
        public Task<ConfigurationUnitDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);
    }
}
