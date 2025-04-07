using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.ServiceCatalogue.Rules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace InfraManager.UI.Web.Controllers.Api.Rules;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RulesController : ControllerBase
{
    private readonly IRuleBLL _ruleBLL;

    public RulesController(IRuleBLL ruleBLL)
    {
        _ruleBLL = ruleBLL;
    }

    [HttpGet]
    public async Task<RuleDetails[]> ListAsync([FromQuery] RuleFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await _ruleBLL.ListAsync(filter, cancellationToken);
    }

    [HttpDelete("{ruleID}")]
    public async Task DeleteAsync([FromRoute] Guid ruleID, CancellationToken cancellationToken = default)
    {
        await _ruleBLL.DeleteAsync(ruleID, cancellationToken);
    }

    [HttpPost]
    public async Task<RuleDetails> InsertAsync([FromBody] RuleData data,
        CancellationToken cancellationToken = default)
    {
        return await _ruleBLL.InsertAsync(data, cancellationToken);
    }

    [HttpPut("{ruleID}")]
    public async Task UpdateAsync([FromRoute] Guid ruleID, [FromBody] RuleData data,
        CancellationToken cancellationToken = default)
    {
        await _ruleBLL.UpdateAsync(ruleID, data, cancellationToken);
    }

    [HttpPost("{ruleID}/Value")]
    public async Task InsertValueAsync([FromRoute] Guid ruleID, [FromBody] RuleValueDetails ruleValueData,
        CancellationToken cancellationToken = default)
    {
        await _ruleBLL.InsertValueAsync(ruleID, ruleValueData, cancellationToken);
    }
    
    [HttpPut("{ruleID}/Value")]
    public async Task UpdateValueAsync([FromRoute] Guid ruleID, [FromBody] RuleValueDetails ruleValueData,
        CancellationToken cancellationToken = default)
    {
        await _ruleBLL.UpdateValueAsync(ruleID, ruleValueData, cancellationToken);
    }
    
    [HttpGet("{ruleID}/Value")]
    public async Task<RuleValueDetails> RuleValueAsync([FromRoute] Guid ruleID, CancellationToken cancellationToken = default)
    {
        return await _ruleBLL.GetRuleValueAsync(ruleID, cancellationToken);
    }
}