using InfraManager.BLL.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.BLL.Catalog;
using InfraManager.DAL.Location;
using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.Location.StorageLocations;

namespace InfraManager.UI.Web.Controllers.BFF.Location;

[Authorize]
[ApiController]
[Route("bff/[controller]")]
public class StorageLocationsController : ControllerBase
{
    private readonly IBasicCatalogBLL<StorageLocation, StorageLocationDetails, Guid, StorageLocationForTable> _catalogBLL;
    private readonly IStorageLocationBLL _storageLocationBLL;
    public StorageLocationsController(IBasicCatalogBLL<StorageLocation, StorageLocationDetails, Guid, StorageLocationForTable> catalogBLL, 
    IStorageLocationBLL storageLocationBLL)
    {
        _catalogBLL = catalogBLL;
        _catalogBLL.SetIncludeItems(c => c.StorageLocationReferences);
        _storageLocationBLL = storageLocationBLL;
    }

    [HttpGet("{id}")]
    public async Task<StorageLocationDetails> GetByIDAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _catalogBLL.GetByIDAsync(id, cancellationToken);
    }

    [HttpGet("list")]
    public async Task<StorageLocationDetails[]> GetListAsync(string search, CancellationToken cancellationToken)
    {
        return await _catalogBLL.GetListAsync(search, cancellationToken);
    }

    [HttpPost("tree")]
    public async Task<LocationTreeNodeDetails[]> GetTreeAsync([FromBody] LocationTreeFilter filter, [FromQuery] Guid storageID, CancellationToken cancellationToken)
    {
        return await _storageLocationBLL.GetTreeNodesByParentIDAsync(filter, storageID, cancellationToken);
    }

    [HttpGet]
    public async Task<StorageLocationDetails[]> GetTableAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken)
    {
        return await _catalogBLL.GetByFilterAsync(filter, cancellationToken);
    }

    [HttpPost]
    public async Task<StorageLocationDetails> AddAsync([FromBody] StorageLocationInsertDetails model, CancellationToken cancellationToken)
    {
        return await _storageLocationBLL.AddAsync(model, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<StorageLocationDetails> UpdateAsync([FromBody] StorageLocationDetails model, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _storageLocationBLL.UpdateAsync(model, id, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _storageLocationBLL.DeleteAsync(id, cancellationToken);
    }

    [HttpGet("table")]
    public async Task<LocationListItem[]> GetLocationTable([FromQuery] StorageFilterLocation filter, CancellationToken cancellationToken)
    {
        return await _storageLocationBLL.GetTableLocationAsync(filter, cancellationToken);
    }

}
