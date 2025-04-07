using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;

namespace InfraManager.UI.Web.Controllers.BFF;

[Authorize]
[ApiController]
[Route("bff/[controller]")]
public class ServiceItemsController : ControllerBase
{
    private readonly IServiceItemAndAttendanceBLL<ServiceItem, ServiceItemDetails, ServiceItemData, ServiceItemColumns> _serviceItemAndAttendanceBLL;

    public ServiceItemsController(IServiceItemAndAttendanceBLL<ServiceItem, ServiceItemDetails, ServiceItemData, ServiceItemColumns> serviceItemAndAttendanceBLL)
    {
        _serviceItemAndAttendanceBLL = serviceItemAndAttendanceBLL;
    }

    [HttpGet("{id}")]
    public async Task<ServiceItemDetails> GetByIDAsync([FromRoute] Guid id, CancellationToken cancellationToken) 
        => await _serviceItemAndAttendanceBLL.GetByIDAsync(id, cancellationToken);
    

    [HttpGet]
    public async Task<ServiceItemDetails[]> GetListAsync([FromQuery] BaseFilter filter, [FromQuery] Guid serviceId, CancellationToken cancellationToken) 
        => await _serviceItemAndAttendanceBLL.GetByServiceIdAsync(serviceId, filter, cancellationToken);
    

    [HttpPost]
    public async Task<Guid> PostAsync([FromBody] ServiceItemData data, CancellationToken cancellationToken) 
        => await _serviceItemAndAttendanceBLL.AddAsync(data, cancellationToken);
    

    [HttpPut("{id::guid}")]
    public async Task<Guid> PutAsync([FromRoute] Guid id
        , [FromBody] ServiceItemData data
        , CancellationToken cancellationToken)
        => await _serviceItemAndAttendanceBLL.UpdateAsync(id, data, cancellationToken);
    

    [HttpDelete("{id}")]
    public async Task Delete([FromRoute] Guid id, CancellationToken cancellationToken) 
        => await _serviceItemAndAttendanceBLL.RemoveAsync(id, cancellationToken);
}