using InfraManager.BLL.Import;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ServiceCatalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace InfraManager.UI.Web.Controllers.BFF.Import.ServiceCatalogue;

[ApiController]
[Authorize]
[Route("bff/[controller]")]
public class ServiceCatalogueConfigurationCSVController : ControllerBase
{
    private readonly IImportApi _service;
    public ServiceCatalogueConfigurationCSVController(IImportApi service)
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
    public async Task<ServiceCatalogueImportCSVConfigurationDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _service.GetConfigurationServiceCatalogueAsync(id, cancellationToken);
    }

    /// <summary>
    /// Метод получает все конфигурации 
    /// </summary>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    [HttpGet]
    public async Task<ServiceCatalogueImportCSVConfigurationDetails[]> GetAsync(CancellationToken cancellationToken = default)
    {
        return await _service.GetConfigurationsServiceCatalogueAsync(cancellationToken);
    }

    /// <summary>
    /// Метод задает конфигурацию
    /// </summary>
    /// <param name="configurationCSVData">модель конфигурации</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    [HttpPost]
    public async Task<Guid?> PostAsync([FromBody] ServiceCatalogueImportCSVConfigurationData configurationCSVData, CancellationToken cancellationToken = default)
    {
        return await _service.SetConfigurationServiceCatalogueAsync(configurationCSVData, cancellationToken);
    }

    /// <summary>
    /// Метод обновляет конфигурацию
    /// </summary>
    /// <param name="configurationCSVData">модель конфигурации</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    [HttpPut("{id}")]
    public async Task PutAsync(Guid id, [FromBody] ServiceCatalogueImportCSVConfigurationData configurationCSVData, CancellationToken cancellationToken = default)
    {
        await _service.UpdateConfigurationServiceCatalogueAsync(id, configurationCSVData, cancellationToken);
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
        await _service.DeleteConfigurationServiceCatalogueAsync(id, cancellationToken);
    }
}
