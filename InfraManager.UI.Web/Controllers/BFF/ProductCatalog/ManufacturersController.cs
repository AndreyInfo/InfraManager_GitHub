using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.Manufactures;
using InfraManager.DAL.Asset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF.ProductCatalogue;

[Authorize]
[ApiController]
[Route("bff/ProductCatalog/Manufacturers")]
public class ManufacturersController : ControllerBase
{
    private readonly IManufacturersBLL _manufacturesBLL;

    public ManufacturersController(IManufacturersBLL manufacturesBLL)
    {
        _manufacturesBLL = manufacturesBLL;
    }

    [HttpGet]
    public async Task<ManufacturerDetails[]> GetAsync([FromQuery] ManufacturersFilter filter
        , [FromQuery] ClientPageFilter clientPageFilter
        , CancellationToken token)
    {
        var pageFilterWithOrder = new ClientPageFilter
        {
            Ascending = clientPageFilter.Ascending,
            OrderByProperty = clientPageFilter.OrderByProperty ?? nameof(Manufacturer.Name),
            Skip =clientPageFilter.Skip,
            Take = clientPageFilter.Take
        };
        return await _manufacturesBLL.GetDetailsPageAsync(filter, pageFilterWithOrder, token);
    }

    [HttpGet("list")]
    public async Task<ManufacturerDetails[]> GetListAsync([FromQuery] ManufacturersFilter filter, CancellationToken token)
        => await _manufacturesBLL.GetPaggingAsync(filter, token);

    [HttpGet("{id}")]
    public async Task<ManufacturerDetails> GetAsync([FromRoute] int id, CancellationToken token)
        => await _manufacturesBLL.DetailsAsync(id, token);
    

    [HttpPost]
    public async Task<ManufacturerDetails> PostAsync([FromBody] ManufacturerData data
        , CancellationToken token)
        => await _manufacturesBLL.AddAsync(data, token);
    

    [HttpPut("{id}")]
    public async Task<ManufacturerDetails> PutAsync([FromRoute] int id
        , [FromBody] ManufacturerData data
        , CancellationToken token)
        => await _manufacturesBLL.UpdateAsync(id, data, token);

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] int id, CancellationToken token)
        => await _manufacturesBLL.DeleteAsync(id, token);
}