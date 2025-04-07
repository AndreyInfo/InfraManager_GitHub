using InfraManager.BLL.Highlighting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Highlighting;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HighlightingController : ControllerBase
{
    private readonly IHighlightingBLL _highlightingBLL;

    public HighlightingController(IHighlightingBLL highlightingBLL)
    {
        _highlightingBLL = highlightingBLL;
    }

    [HttpGet]
    public async Task<HighlightingDetails[]> ListAsync(CancellationToken cancellationToken = default)
        => await _highlightingBLL.GetAllDetailsArrayAsync(cancellationToken);

    [HttpPost]
    public async Task<HighlightingDetails> AddAsync([FromBody] HighlightingData data,
        CancellationToken cancellationToken = default)
        => await _highlightingBLL.AddAsync(data, cancellationToken);

    [HttpPut("{id}")]
    public async Task<HighlightingDetails> UpdateAsync([FromRoute] Guid id, [FromBody] HighlightingData data,
        CancellationToken cancellationToken = default)
        => await _highlightingBLL.UpdateAsync(id, data, cancellationToken);

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        => await _highlightingBLL.DeleteAsync(id, cancellationToken);
}
