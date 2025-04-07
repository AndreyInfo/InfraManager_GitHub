using IM.Core.Import.BLL.Interface.Import;
using InfraManager.BLL.Import;
using InfraManager.DAL.Import;
using InfraManager.ServiceBase.WebApiModes;
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
    public class ImportController : ControllerBase
    {
        private IImportBLL _service;

        public ImportController(IImportBLL service)
        {
            _service = service;
        }

       
        /// <summary>
        /// Метод отправляет вкладку "Общая"
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        [HttpGet("main/{id}")]
        public async Task<ImportMainTabDetails> GetMainTabAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _service.GetMainDetailsAsync(id, cancellationToken);
        }

        /// <summary>
        /// Метод задает вкладку "Общая"
        /// </summary>
        /// <param name="mainTabDetails">модель главной вкладки</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        [HttpPost("main")]
        public async Task<Guid> CreateMainTabAsync(ImportMainTabDetails mainTabDetails, CancellationToken cancellationToken = default)
        {
             return await _service.CreateMainDetailsAsync(mainTabDetails, cancellationToken);
        }

        /// <summary>
        /// Метод изменяет вкладку "Общая"
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="mainTabDetails">модель главной вкладки</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        [HttpPut("main/{id}")]
        public async Task UpdateMainTabAsync(Guid id, ImportMainTabDetails mainTabDetails, CancellationToken cancellationToken = default)
        {
            await _service.UpdateMainDetailsAsync(id, mainTabDetails, cancellationToken);
        }

        /// <summary>
        /// Метод удаляет задачу
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        [HttpDelete("{id}")]
        public async Task<DeleteDetails> DeleteTaskAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _service.DeleteTaskAsync(id, cancellationToken);
        }

        /// <summary>
        /// Метод получает все задачи
        /// </summary>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> список задач </returns>
        [HttpGet]
        public async Task<ImportTasksDetails[]> GetTasksAsync(CancellationToken cancellationToken)
        { 
            return await _service.GetImportTasksAsync(cancellationToken);
        }



        /// <summary>
        /// Метод получает вкладку "Дополнительно"
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> вкладка Дополнительно </returns>
        [HttpGet("additional/{id}")]
        public async Task<AdditionalTabDetails> GetAdditionalTabAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _service.GetAdditionalDetailsAsync(id, cancellationToken);
        }

        /// <summary>
        /// Метод обновляет вкладку "Дополнительно"
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="settings">модель вкладки Дополнительно</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        [HttpPut("additional/{id}")]
        public async Task PutAdditionalTabAsync(Guid id,[FromBody]AdditionalTabData settings, CancellationToken cancellationToken = default)
        {
            await _service.UpdateAdditionalDetailsAsync(id, settings, cancellationToken);
        }
    }
}
