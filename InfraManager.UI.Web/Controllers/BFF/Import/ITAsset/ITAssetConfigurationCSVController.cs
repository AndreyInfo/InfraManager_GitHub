using InfraManager.BLL.Import;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ITAsset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Import.ITAsset;

[ApiController]
[Authorize]
[Route("bff/[controller]")]
public class ITAssetConfigurationCSVController : ControllerBase
{
    private readonly IImportApi _service;
    public ITAssetConfigurationCSVController(IImportApi service)
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
    public async Task<ITAssetImportCSVConfigurationDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _service.GetConfigurationITAssetAsync(id, cancellationToken);
    }

    /// <summary>
    /// Метод получает все конфигурации 
    /// </summary>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    [HttpGet]
    public async Task<ITAssetImportCSVConfigurationDetails[]> GetAsync(CancellationToken cancellationToken = default)
    {
        return await _service.GetConfigurationsITAssetAsync(cancellationToken);
    }

    /// <summary>
    /// Метод задает конфигурацию
    /// </summary>
    /// <param name="configurationCSVData">модель конфигурации</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    [HttpPost]
    public async Task<Guid?> PostAsync([FromBody] ITAssetImportCSVConfigurationData configurationCSVData, CancellationToken cancellationToken = default)
    {
        return await _service.SetConfigurationITAssetAsync(configurationCSVData, cancellationToken);
    }

    /// <summary>
    /// Метод обновляет конфигурацию
    /// </summary>
    /// <param name="configurationCSVData">модель конфигурации</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    [HttpPut("{id}")]
    public async Task PutAsync(Guid id, [FromBody] ITAssetImportCSVConfigurationData configurationCSVData, CancellationToken cancellationToken = default)
    {
        await _service.UpdateConfigurationITAssetAsync(id, configurationCSVData, cancellationToken);
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
        await _service.DeleteConfigurationITAssetAsync(id, cancellationToken);
    }

    /// <summary>
    /// Метод получает список типов из каталога продуктов
    /// </summary>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    [HttpGet("types")]
    public async Task<ProductCatalogTypeDetails[]> GetTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _service.GetTypesAsync(cancellationToken);
    }
}
