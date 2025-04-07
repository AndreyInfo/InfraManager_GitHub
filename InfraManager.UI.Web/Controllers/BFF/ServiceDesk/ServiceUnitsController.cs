using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.Catalog;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Threading;
using InfraManager.BLL.ServiceDesk.ServiceUnits;
using InfraManager.DAL.ServiceDesk;
using InfraManager.BLL.ServiceCatalogue;

namespace InfraManager.UI.Web.Controllers.BFF.GroupQueue;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ServiceUnitsController : ControllerBase
{
    private readonly IBasicCatalogBLL<ServiceUnit, ServiceUnitDetails, Guid, ServiceUnitColumns> _basicCatalogBLL;
    private readonly IServiceUnitBLL _serviceUnitBLL;
    public ServiceUnitsController(IBasicCatalogBLL<ServiceUnit, ServiceUnitDetails, Guid, ServiceUnitColumns> basicCatalogBLL,
        IServiceUnitBLL serviceUnitBLL)
    {
        _basicCatalogBLL = basicCatalogBLL;
        _basicCatalogBLL.SetIncludeItems(c=> c.ResponsibleUser);
        _basicCatalogBLL.SetIncludeItems(c=> c.OrganizationItemGroups);
        _serviceUnitBLL = serviceUnitBLL;
    }


    [HttpGet("{id}")]
    public async Task<ServiceUnitDetails> GetByIDAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _serviceUnitBLL.GetByIDAsync(id, cancellationToken);
    }


    [HttpGet]
    public async Task<ServiceUnitDetails[]> GetListAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken)
    {
        return await _basicCatalogBLL.GetByFilterAsync(filter, cancellationToken);
    }

    [HttpPost]
    public async Task<ServiceUnitDetails> AddAsync([FromBody] ServiceUnitInsertDetails model, CancellationToken cancellationToken)
    {
        return await _serviceUnitBLL.AddAsync(model, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ServiceUnitDetails> UpdateAsync([FromBody] ServiceUnitDetails model, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _serviceUnitBLL.UpdateAsync(model, id, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _basicCatalogBLL.RemoveAsync(id, cancellationToken);
    }
}
