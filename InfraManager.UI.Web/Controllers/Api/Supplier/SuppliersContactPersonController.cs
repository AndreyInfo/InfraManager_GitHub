using InfraManager.BLL.Suppliers.SupplierContactPerson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Supplier;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SuppliersContactPersonController : ControllerBase
{
    private readonly ISupplierContactPersonBLL _supplierContactPersonBLL;

    public SuppliersContactPersonController(ISupplierContactPersonBLL supplierContactPersonBLL)
    {
        _supplierContactPersonBLL = supplierContactPersonBLL;
    }

    [HttpGet("{id}")]
    public async Task<SupplierContactPersonDetails> GetAsync(
        [FromRoute] Guid id, CancellationToken cancellationToken = default)
        => await _supplierContactPersonBLL.DetailsAsync(id, cancellationToken);

    [HttpGet]
    public async Task<SupplierContactPersonDetails[]> GetDetailsAsync(
        [FromQuery] SupplierContactPersonFilter filter, CancellationToken cancellationToken = default)
        => await _supplierContactPersonBLL.GetListAsync(filter, cancellationToken);

    [HttpPost]
    public async Task<SupplierContactPersonDetails> AddAsync(
        [FromBody] SupplierContactPersonData data, CancellationToken cancellationToken)
        => await _supplierContactPersonBLL.AddAsync(data, cancellationToken);

    [HttpPut("{id}")]
    public async Task<SupplierContactPersonDetails> UpdateAsync(
        [FromRoute] Guid id, [FromBody] SupplierContactPersonData data, CancellationToken cancellationToken)
        => await _supplierContactPersonBLL.UpdateAsync(id, data, cancellationToken);

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        => await _supplierContactPersonBLL.DeleteAsync(id, cancellationToken);
}