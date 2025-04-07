using InfraManager.BLL.Import;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ITAsset;
using InfraManager.ServiceBase.ScheduleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Import.ITAsset;

[ApiController]
[Authorize]
[Route("bff/[controller]")]
public class ITAssetController : ControllerBase
{
    private readonly IImportApi _service;
    public ITAssetController(IImportApi service)
    {
        _service = service;
    }

    /// <summary>
    /// Метод запускает задачу импорта
    /// </summary>
    /// <param name="importTasksRequest">модель задачи из шедулера</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    [HttpPost("start")]
    public async Task StartImportAsync(ImportTaskRequest importTasksRequest, CancellationToken cancellationToken = default)
    {
        await _service.StartImportITAssetAsync(importTasksRequest, cancellationToken);
    }

    /// <summary>
    /// Метод получает задачу импорта по идентификатору
    /// </summary>
    /// <param name="id">идентификатор задачи</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> задача импорта </returns>
    [HttpGet("{id}")]
    public async Task<ITAssetImportSettingDetails> GetTaskAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _service.GetImportTaskByIDITAssetAsync(id, cancellationToken);
    }

    /// <summary>
    /// Метод создает задачу
    /// </summary>
    /// <param name="data">модель задачи импорта сервисов</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> идентификатор созданной задачи </returns>
    [HttpPost]
    public async Task<Guid> CreateTaskAsync(ITAssetImportSettingData data, CancellationToken cancellationToken = default)
    {
        return await _service.CreateImportTaskITAssetAsync(data, cancellationToken);
    }

    /// <summary>
    /// Метод изменяет задачу
    /// </summary>
    /// <param name="id">идентификатор задачи</param>
    /// <param name="data">модель задачи</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    [HttpPut("{id}")]
    public async Task UpdateTaskAsync(Guid id, ITAssetImportSettingData data, CancellationToken cancellationToken = default)
    {
        await _service.UpdateImportTaskITAssetAsync(id, data, cancellationToken);
    }

    /// <summary>
    /// Метод удаляет задачу
    /// </summary>
    /// <param name="id">идентификатор задачи</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    [HttpDelete("{id}")]
    public async Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _service.DeleteImportTaskITAssetAsync(id, cancellationToken);
    }

    /// <summary>
    /// Метод получает все задачи
    /// </summary>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> список задач </returns>
    [HttpGet]
    public async Task<ITAssetImportSettingDetails[]> GetTasksAsync(CancellationToken cancellationToken)
    {
        return await _service.GetAllImportTasksITAssetAsync(cancellationToken);
    }
}
