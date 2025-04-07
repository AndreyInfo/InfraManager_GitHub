using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.Location.Subnets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace InfraManager.UI.Web.Controllers.BFF.Location;

[Authorize]
[ApiController]
[Route("bff/[controller]")]
public class SubnetsController : ControllerBase
{
    private readonly ISubnetBLL _subnetBLL;

    public SubnetsController(ISubnetBLL subnetBLL)
    {
        _subnetBLL = subnetBLL;
    }

    [HttpGet("{id}")]
    public async Task<SubnetDetails> GetByIDAsync([FromRoute] int id,  CancellationToken cancellationToken)
    {
        return await _subnetBLL.GetByIDAsync(id, cancellationToken);
    }

    [HttpGet]
    public async Task<SubnetDetails[]> GetListByBuildingIDAsync([FromQuery] int buildingID, [FromQuery] string search, CancellationToken cancellationToken)
    {
        return await _subnetBLL.GetSubnetsByBuildingIDAsync(buildingID, search, cancellationToken);
    }

    [HttpPost]
    public async Task<int> AddAsync([FromBody] SubnetDetails model, CancellationToken cancellationToken)
    {
        return await _subnetBLL.AddAsync(model, cancellationToken);
    }

    [HttpPut]
    public async Task<int> UpdateAsync([FromBody] SubnetDetails model, CancellationToken cancellationToken)
    {
        return await _subnetBLL.UpdateAsync(model, cancellationToken);
    }


    [HttpDelete]
    public async Task RemoveAsync([FromBody] DeleteModel<int>[] models, CancellationToken cancellationToken)
    {
        await _subnetBLL.DeleteAsync(models, cancellationToken);
    }

}
