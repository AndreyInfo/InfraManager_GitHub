using InfraManager.BLL.ProductCatalogue.ModelCharacteristics;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ModelCharacteristicsController : ControllerBase
{
    private readonly IEntityCharacteristicsBLL _entityCharacteristicsBLL;

    public ModelCharacteristicsController(IEntityCharacteristicsBLL entityCharacteristicsBLL)
    {
        _entityCharacteristicsBLL = entityCharacteristicsBLL;
    }

    [HttpGet("{templateID}/{id}")]
    public async Task<EntityCharacteristicsDetailsBase> GetAsync([FromRoute] Guid id
        , [FromRoute] ProductTemplate templateID
        , CancellationToken cancellationToken = default)
        => await _entityCharacteristicsBLL.GetAsync(id, templateID, cancellationToken);

    [HttpPut("{templateID}/{id}")]
    public async Task<EntityCharacteristicsDetailsBase> UpdateAsync([FromRoute] Guid id
        , [FromRoute] ProductTemplate templateID
        , [FromBody] EntityCharacteristicsDataBase data
        , CancellationToken cancellationToken = default)
        => await _entityCharacteristicsBLL.UpdateAsync(id, templateID, data, cancellationToken);
}
