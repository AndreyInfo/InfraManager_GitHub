using InfraManager.BLL.ServiceDesk.Solutions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SolutionsController : ControllerBase
{
    private readonly ISolutionBLL _solutionBLL;

    public SolutionsController(ISolutionBLL solutionBLL)
    {
        _solutionBLL = solutionBLL;
    }

    [HttpGet("{id:guid}")]
    public Task<SolutionDetails> GetByIDAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        => _solutionBLL.DetailsAsync(id, cancellationToken);


    [HttpGet]
    public Task<SolutionDetails[]> GetListAsync(CancellationToken cancellationToken)
        => _solutionBLL.GetAllDetailsArrayAsync(cancellationToken);

    [HttpPost]
    public Task<SolutionDetails> PostAsync([FromBody] SolutionData data, CancellationToken cancellationToken)
        => _solutionBLL.AddAsync(data, cancellationToken);


    [HttpPut("{id:guid}")]
    public Task<SolutionDetails> PutAsync([FromRoute] Guid id
        , [FromBody] SolutionData data
        , CancellationToken cancellationToken)
        => _solutionBLL.UpdateAsync(id, data, cancellationToken);


    [HttpDelete("{id:guid}")]
    public Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        => _solutionBLL.DeleteAsync(id, cancellationToken);
}
