using InfraManager.BLL.Import;
using InfraManager.DAL.Import.CSV;
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
    public class ImportCSVController : ControllerBase
    {

        private IImportCSVBLL _service;

        public ImportCSVController(IImportCSVBLL service)
        {
            _service = service;
        }
        /// <summary>
        /// Метод получает список всех CSV конфигурации 
        /// </summary>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> список CSV конфигурации </returns>
        [HttpGet]
        public async Task<CSVConfigurationTableAPI[]> GetConfigurationTable(CancellationToken cancellationToken = default)
        {
            return await _service.GetConfigurationTableAsync(cancellationToken);
        }

        /// <summary>
        /// Метод получает путь для файла CSV 
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> путь файла CSV </returns>
        [HttpGet("path/{id}")]
        public async Task<string> GetPathTable(Guid id, CancellationToken cancellationToken = default)
        {
            return await _service.GetPathAsync(id, cancellationToken);
        }

        /// <summary>
        /// Метод обновляет путь CSV файла
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="path">путь до файла</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> список CSV конфигурации </returns>
        [HttpPut("path/{id}")]
        public async Task UpdatePathTable(Guid id,[FromBody] String path, CancellationToken cancellationToken = default)
        {
            await _service.UpdatePathAsync(id, path, cancellationToken);
        }
    }
}
