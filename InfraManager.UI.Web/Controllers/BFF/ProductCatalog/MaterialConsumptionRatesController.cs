using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.MaterialConsumptionRates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue;

[Authorize]
[ApiController]
[Route("bff/ProductCatalog/MaterialConsumptionRates")]
public class MaterialConsumptionRatesController:ControllerBase
{
    private readonly IMaterialConsumptionRateBLL _materialConsumptionRateBLL;

    public MaterialConsumptionRatesController(IMaterialConsumptionRateBLL materialConsumptionRateBLL)
    {
        _materialConsumptionRateBLL = materialConsumptionRateBLL;
    }

    [HttpGet]
    public Task<MaterialConsumptionRateOutputDetails[]> GetAsync([FromQuery] MaterialConsumptionRateFilter filter,
        [FromQuery] ClientPageFilter clientPageFilter, CancellationToken token)
    {
        return _materialConsumptionRateBLL.GetDetailsPageAsync(filter, clientPageFilter, token);
    }

    [HttpGet("{id}")]
    public Task<MaterialConsumptionRateOutputDetails> GetAsync(Guid id, CancellationToken token)
    {
        return _materialConsumptionRateBLL.DetailsAsync(id, token);
    }

    [HttpPost]
    public Task<MaterialConsumptionRateOutputDetails> PostAsync([FromBody] MaterialConsumptionRateInputDetails inputDetails, CancellationToken token)
    {
        return _materialConsumptionRateBLL.AddAsync(inputDetails, token);
    }

    [HttpPut("{id}")]
    public Task<MaterialConsumptionRateOutputDetails> PutAsync(Guid id, [FromBody]MaterialConsumptionRateInputDetails inputDetails, CancellationToken token)
    {
        return _materialConsumptionRateBLL.UpdateAsync(id, inputDetails, token);
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id, CancellationToken token)
    {
        return _materialConsumptionRateBLL.DeleteAsync(id, token);
    }
}