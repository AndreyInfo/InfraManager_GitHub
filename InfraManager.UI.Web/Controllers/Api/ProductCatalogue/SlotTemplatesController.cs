using InfraManager.BLL.ProductCatalogue.Slots;
using InfraManager.BLL.ProductCatalogue.SlotTemplates;
using InfraManager.DAL.Asset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SlotTemplatesController : ControllerBase
{
    private readonly ISlotBaseBLL<SlotTemplate, SlotTemplateData, SlotTemplateDetails, SlotTemplateColumns> _slotBaseBLL;

    public SlotTemplatesController(ISlotBaseBLL<SlotTemplate, SlotTemplateData, SlotTemplateDetails, SlotTemplateColumns> slotBaseBLL)
    {
        _slotBaseBLL = slotBaseBLL;
    }

    [HttpGet("{objectID}/{number}")]
    public async Task<SlotTemplateDetails> GetAsync([FromRoute] Guid objectID
        , [FromRoute] int number
        , CancellationToken cancellationToken = default)
    {
        var key = new SlotBaseKey(objectID, number);
        return await _slotBaseBLL.DetailsAsync(key, cancellationToken);
    }

    [HttpGet]
    public async Task<SlotTemplateDetails[]> GetDetailsAsync([FromQuery] SlotBaseFilter filter, CancellationToken cancellationToken = default)
        => await _slotBaseBLL.GetListAsync(filter, cancellationToken);

    [HttpPost]
    public async Task<SlotTemplateDetails> AddAsync([FromBody] SlotTemplateData data, CancellationToken cancellationToken)
        => await _slotBaseBLL.AddAsync(data, cancellationToken);

    [HttpPut("{objectID}/{number}")]
    public async Task<SlotTemplateDetails> UpdateAsync([FromRoute] Guid objectID
        , [FromRoute] int number
        , [FromBody] SlotTemplateData data
        , CancellationToken cancellationToken)
    {
        var key = new SlotBaseKey(objectID , number);
        return await _slotBaseBLL.UpdateAsync(key, data, cancellationToken);
    }

    [HttpDelete("{objectID}/{number}")]
    public async Task DeleteAsync([FromRoute] Guid objectID
        , [FromRoute] int number
        , CancellationToken cancellationToken)
    {
        var key = new SlotBaseKey(objectID, number);
        await _slotBaseBLL.DeleteAsync(key, cancellationToken);
    }
}
