using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.PortTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PortTemplatesController : ControllerBase
{
    private readonly IPortTemplatesBLL _portTemplatesBLL;

    public PortTemplatesController(IPortTemplatesBLL portTemplatesBLL)
    {
        _portTemplatesBLL = portTemplatesBLL;
    }

    [HttpGet]
    public Task<PortTemplatesDetails[]> GetAsync([FromQuery] PortTemplatesFilter filter, CancellationToken token)
    {
        return _portTemplatesBLL.GetListAsync(filter, token);
    }

    [HttpGet("{objectID}/{portNumber}")]
    public Task<PortTemplatesDetails> GetAsync(Guid objectID, int portNumber, CancellationToken token)
    {
        var key = new PortTemplatesKey(objectID, portNumber);
        return _portTemplatesBLL.DetailsAsync(key, token);
    }

    [HttpPost]
    public Task<PortTemplatesDetails> PostAsync([FromBody] PortTemplatesData data, CancellationToken token)
    {
        return _portTemplatesBLL.AddAsync(data, token);
    }

    [HttpPut("{objectID}/{portNumber}")]
    public Task<PortTemplatesDetails> PutAsync(Guid objectID, int portNumber, [FromBody] PortTemplatesData data, CancellationToken token)
    {
        var key = new PortTemplatesKey(objectID, portNumber);
        return _portTemplatesBLL.UpdateAsync(key, data, token);
    }

    [HttpDelete("{objectID}/{portNumber}")]
    public Task DeleteAsync(Guid objectID, int portNumber, CancellationToken token)
    {
        var key = new PortTemplatesKey(objectID, portNumber);
        return _portTemplatesBLL.DeleteAsync(key, token);
    }
}
