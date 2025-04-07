using InfraManager.BLL.Configuration.Technologies;
using InfraManager.BLL.Technologies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Settings.Configuration;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TechnologyTypeController : ControllerBase
{
    private readonly ITechnologyTypeBLL _technologyTypesBLL;

    public TechnologyTypeController(ITechnologyTypeBLL technologyTypesBLL)
    {
        _technologyTypesBLL = technologyTypesBLL;
    }


    [HttpGet("{id:int}")]
    public async Task<TechnologyTypeDetails> GetAsync([FromRoute] int id
        , CancellationToken cancellationToken = default)
        => await _technologyTypesBLL.DetailsAsync(id, cancellationToken);


    [HttpGet]
    public async Task<TechnologyTypeDetails[]> GetListAsync([FromQuery] TechnologyTypeFilter filter
        , CancellationToken cancellationToken = default)
        => await _technologyTypesBLL.GetDetailsArrayAsync(filter, cancellationToken);


    [HttpPost]
    public async Task<TechnologyTypeDetails> PostAsync([FromBody] TechnologyTypeData technologyType
        , CancellationToken cancellationToken = default)
        => await _technologyTypesBLL.AddAsync(technologyType, cancellationToken);


    [HttpPut("{id}")]
    public async Task<TechnologyTypeDetails> PutAsync([FromRoute] int id
        , [FromBody] TechnologyTypeData technologyType
        , CancellationToken cancellationToken = default)
        => await _technologyTypesBLL.UpdateAsync(id, technologyType, cancellationToken);



}
