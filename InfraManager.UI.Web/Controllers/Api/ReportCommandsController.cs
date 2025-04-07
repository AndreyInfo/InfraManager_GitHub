using InfraManager.BLL.ReportsForCommand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportCommandsController : Controller
{
    private readonly IReportsForCommandBLL _reportsForCommand;

    public ReportCommandsController(IReportsForCommandBLL reportsForCommand)
    {
        _reportsForCommand = reportsForCommand;
    }

    [HttpGet("{id}")]
    public async Task<ReportForCommandDetails> GetAsync([FromRoute] byte id, CancellationToken cancellationToken = default)
        => await _reportsForCommand.GetAsync(id, cancellationToken);

    [HttpGet]
    public async Task<ReportForCommandDetails[]> GetDetailsAsync(CancellationToken cancellationToken = default)
        => await _reportsForCommand.GetListAsync(cancellationToken);

    [HttpPost]
    public async Task<ReportForCommandDetails> AddAsync([FromBody] ReportForCommandData data, CancellationToken cancellationToken = default)
        =>  await _reportsForCommand.AddAsync(data, cancellationToken);

    [HttpPut("{id}")]
    public async Task<ReportForCommandDetails> UpdateAsync([FromRoute] byte id, 
        [FromBody] ReportForCommandData data, CancellationToken cancellationToken = default)
    => await _reportsForCommand.UpdateAsync(id, data, cancellationToken);

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
        => await _reportsForCommand.DeleteAsync(id, cancellationToken);
}