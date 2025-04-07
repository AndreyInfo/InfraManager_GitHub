using InfraManager.BLL.ProductCatalogue.Slots;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using InfraManager.DAL.Asset;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SlotsController : ControllerBase
{
    private readonly ISlotBaseBLL<Slot, SlotData, SlotDetails, SlotColumns> _slotBaseBLL;

    public SlotsController(ISlotBaseBLL<Slot, SlotData, SlotDetails, SlotColumns> slotBaseBLL)
    {
        _slotBaseBLL = slotBaseBLL;
    }

    [HttpGet("{objectID}/{number}")]
    public async Task<SlotDetails> GetAsync([FromRoute] Guid objectID
        , [FromRoute] int number
        , CancellationToken cancellationToken = default)
    {
        var key = new SlotBaseKey(objectID, number);
        return await _slotBaseBLL.DetailsAsync(key, cancellationToken);
    }

    [HttpGet]
    public async Task<SlotDetails[]> GetDetailsAsync([FromQuery] SlotBaseFilter filter, CancellationToken cancellationToken = default)
        => await _slotBaseBLL.GetListAsync(filter, cancellationToken);

    [HttpPost]
    public async Task<SlotDetails> AddAsync([FromBody] SlotData data, CancellationToken cancellationToken)
        => await _slotBaseBLL.AddAsync(data, cancellationToken);

    [HttpPut("{objectID}/{number}")]
    public async Task<SlotDetails> UpdateAsync([FromRoute] Guid objectID
        , [FromRoute] int number
        , [FromBody] SlotData data
        , CancellationToken cancellationToken)
    {
        var key = new SlotBaseKey(objectID, number);
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
