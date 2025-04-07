﻿using InfraManager.BLL.Asset.ActivePort;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Asset;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivePortsController : ControllerBase
{
    private readonly IActivePortBLL _activePortBLL;

    public ActivePortsController(IActivePortBLL activePort)
    {
        _activePortBLL = activePort;
    }

    [HttpGet("{id}")]
    public async Task<ActivePortDetails> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        return await _activePortBLL.DetailsAsync(id, cancellationToken);
    }

    [HttpGet]
    public async Task<ActivePortDetails[]> GetDetailsAsync([FromQuery] ActivePortFilter filter, CancellationToken cancellationToken = default)
        => await _activePortBLL.GetListAsync(filter, cancellationToken);

    [HttpPost]
    public async Task<ActivePortDetails> AddAsync([FromBody] ActivePortData data, CancellationToken cancellationToken)
        => await _activePortBLL.AddAsync(data, cancellationToken);

    [HttpPut("{id}")]
    public async Task<ActivePortDetails> UpdateAsync([FromRoute] int id,
        [FromBody] ActivePortData data, CancellationToken cancellationToken)
    {
        return await _activePortBLL.UpdateAsync(id, data, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _activePortBLL.DeleteAsync(id, cancellationToken);
    }

    [HttpGet("countPorts/{id}")]
    public async Task<int> GetCountPortsAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        return await _activePortBLL.GetCountPortsAsync(id, cancellationToken);
    }
}

