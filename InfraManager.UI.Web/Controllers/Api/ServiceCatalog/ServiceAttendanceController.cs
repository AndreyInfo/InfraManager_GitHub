using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceAttendances;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ServiceAttendanceController : ControllerBase
{
    private readonly IServiceItemAndAttendanceBLL<ServiceAttendance, ServiceAttendanceDetails, ServiceAttendanceData, ServiceAttendanceForTable> _serviceItemAndAttendanceBLL;
    private readonly ISupportLineBLL _supportLineBLL;

    public ServiceAttendanceController(IServiceItemAndAttendanceBLL<ServiceAttendance, ServiceAttendanceDetails, ServiceAttendanceData, ServiceAttendanceForTable> serviceItemAndAttendanceBLL, 
        ISupportLineBLL supportLineBLL)
    {
        _serviceItemAndAttendanceBLL = serviceItemAndAttendanceBLL;
        _supportLineBLL = supportLineBLL;
    }

    [HttpGet("{id}")]
    public async Task<ServiceAttendanceDetails> GetByIDAsync([FromRoute] Guid id, CancellationToken cancellationToken) 
        => await _serviceItemAndAttendanceBLL.GetByIDAsync(id, cancellationToken);

    [HttpGet]
    public async Task<ServiceAttendanceDetails[]> GetListAsync([FromQuery] BaseFilter filter, Guid serviceId, CancellationToken cancellationToken)
        => await _serviceItemAndAttendanceBLL.GetByServiceIdAsync(serviceId, filter, cancellationToken);

    [HttpPost]
    public async Task<Guid> PostAsync([FromBody] ServiceAttendanceData data, CancellationToken cancellationToken) 
        => await _serviceItemAndAttendanceBLL.AddAsync(data, cancellationToken);
    

    [HttpPut("{id::guid}")]
    public async Task<Guid> PutAsync([FromRoute] Guid id, 
        [FromBody] ServiceAttendanceData model
        , CancellationToken cancellationToken) 
        => await _serviceItemAndAttendanceBLL.UpdateAsync(id, model, cancellationToken);


    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        => await _serviceItemAndAttendanceBLL.RemoveAsync(id, cancellationToken);
    

    [HttpGet("{id}/responsible")]
    public async Task<SupportLineResponsibleDetails[]> GetResponsibleAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _supportLineBLL.GetResponsibleObjectLineAsync(id, ObjectClass.ServiceAttendance, cancellationToken);
    }
}