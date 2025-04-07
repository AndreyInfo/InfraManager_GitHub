using InfraManager.BLL.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Import
{
    [ApiController]
    [Authorize]
    [Route("bff/[controller]")]
    public class ConfigurationCSVController : ControllerBase
    {
        private IConfigurationCSVBLL _service;
        public ConfigurationCSVController(IConfigurationCSVBLL service)
        {
            _service = service;
        }
        /// <summary>
        /// Метод получает конфигурацию 
        /// </summary>
        /// <param name="id">идентификатор конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        [HttpGet("{id}")]
        public async Task<ConfigurationCSVDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _service.GetConfigurationAsync(id, cancellationToken);
        }

        /// <summary>
        /// Метод задает конфигурацию
        /// </summary>
        /// <param name="configurationCSVDetails">модель конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        [HttpPost]
        public async Task PostAsync([FromBody] ConfigurationCSVData configurationCSVDetails, CancellationToken cancellationToken = default)
        {
            await _service.SetConfigurationAsync(configurationCSVDetails, cancellationToken);
        }

        /// <summary>
        /// Метод обновляет конфигурацию
        /// </summary>
        /// <param name="configurationCSVDetails">модель конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        [HttpPut("{id}")]
        public async Task PutAsync(Guid id, [FromBody] ConfigurationCSVData configurationCSVDetails, CancellationToken cancellationToken = default)
        {
            await _service.UpdateConfigurationAsync(id,configurationCSVDetails, cancellationToken);
        }

        /// <summary>
        /// Метод удаляет конфигурацию
        /// </summary>
        /// <param name="id">идентификатор конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        [HttpDelete("{id}")]
        public async Task DeteleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _service.DeleteConfigurationAsync(id, cancellationToken);
        }
    }
}
