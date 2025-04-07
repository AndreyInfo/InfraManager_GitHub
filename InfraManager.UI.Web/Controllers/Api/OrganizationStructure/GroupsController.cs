using InfraManager.BLL.Asset;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.BLL.OrganizationStructure.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk;

namespace InfraManager.UI.Web.Controllers.Api.GroupQueue;

[Authorize]
[ApiController]
[Route("api/groups/")]
public class GroupsController : Controller
{
    private readonly IGroupBLL _groupBLL;
    private readonly IGroupWorkloadBLL _groupWorkloadBLL;
    public GroupsController(
        IGroupBLL groupBLL,
        IGroupWorkloadBLL groupWorkloadBLL)
    {
        _groupBLL = groupBLL;
        _groupWorkloadBLL = groupWorkloadBLL;
    }

    [HttpGet("{id}")]
    public async Task<GroupDetails> GetDetailsAsync(Guid id)
    {
        return await _groupBLL.DetailsAsync(id);
    }

    [HttpDelete("{id}")]
    public async Task RemoveAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _groupBLL.DeleteAsync(id, cancellationToken);
    }


    [HttpPost]
    public async Task<Guid> AddAsync([FromBody] GroupData model, CancellationToken cancellationToken)
    {
        return await _groupBLL.AddAsync(model, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<Guid> UpdateAsync([FromBody] GroupDetails model, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _groupBLL.UpdateAsync(model, id, cancellationToken);
    }

    [HttpPost("reports/workload")]
    public async Task<GroupWorkloadListItem[]> GetGroupWorkloadReportAsync(
        [FromBody] WorkloadListData data,
        [FromQuery] ClientPageFilter pageBy,
        CancellationToken cancellationToken = default)
    {
        return await _groupWorkloadBLL.GetGroupWorkloadReportAsync(data, pageBy, cancellationToken);
    }
}