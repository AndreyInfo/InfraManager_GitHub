using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk.Problems;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Problems;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProblemCausesController : ControllerBase
{
    private readonly ILookupBLL<ProblemCauseDetails, ProblemCauseDetails, ProblemCauseData, Guid> _service;
    private readonly IProblemCauseBLL _problemBLL;
    public ProblemCausesController(
        ILookupBLL<ProblemCauseDetails, ProblemCauseDetails, ProblemCauseData, Guid> service, IProblemCauseBLL problemBLL)
    {
        _service = service;
        _problemBLL = problemBLL;
    }

    [HttpGet]
    public async Task<ProblemCauseDetails[]> ListAsync([FromQuery] string search, CancellationToken cancellationToken = default)
    {
        return await _problemBLL.GetListAsync(search, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<ProblemCauseDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return await _service.FindAsync(id, cancellationToken);
    }


    [HttpPut("{id}")]
    public async Task<ProblemCauseDetails> UpdateAsync([FromRoute] Guid id, [FromBody] ProblemCauseData model, CancellationToken cancellationToken = default)
    {
        return await _service.UpdateAsync(id, model, cancellationToken);
    }

    [HttpPost]
    public async Task<ProblemCauseDetails> AddAsync([FromBody] ProblemCauseData model, CancellationToken cancellationToken = default)
    {
        return await _service.AddAsync(model, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        await _service.DeleteAsync(id, cancellationToken);
    }
}
