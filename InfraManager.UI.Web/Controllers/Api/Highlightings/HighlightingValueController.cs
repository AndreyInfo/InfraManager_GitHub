using InfraManager.BLL.Highlighting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Highlightings;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HighlightingValueController : ControllerBase
{
    private readonly IHighlightingBLL _highlightingBLL;

    public HighlightingValueController(IHighlightingBLL highlightingBLL)
    {
        _highlightingBLL = highlightingBLL;
    }

    [HttpGet("{id}")]
    public async Task<HighlightingConditionDetails[]> ListAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        => await _highlightingBLL.ListValueAsync(id, cancellationToken);

    [HttpPost]
    public async Task AddAsync([FromBody] HighlightingConditionData data,
        CancellationToken cancellationToken = default)
        => await _highlightingBLL.AddValueAsync(data, cancellationToken);

    [HttpPut("{id}")]
    public async Task UpdateAsync([FromRoute] Guid id, [FromBody] HighlightingConditionData data,
        CancellationToken cancellationToken = default)
        => await _highlightingBLL.UpdateValueAsync(id, data, cancellationToken);

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        => await _highlightingBLL.DeleteValueAsync(id, cancellationToken);
}
