using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.AccessManagement.AccessPermissions;
using InfraManager.DAL.AccessManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Access
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccessPermissionController : ControllerBase
    {
        private readonly IAccessPermissionBLL _accessPermissionBLL;
        public AccessPermissionController(IAccessPermissionBLL accessPermissionBLL)
        {
            _accessPermissionBLL = accessPermissionBLL;
        }

        [HttpGet]
        public  async Task<AccessPermissionDetails[]> GetListAsync([FromQuery] BaseFilterWithClassIDAndID<Guid> filter
            , CancellationToken cancellationToken)
            => await _accessPermissionBLL.GetDataTableAsync(filter, cancellationToken);

        [HttpGet("{id::guid}")]
        public async Task<AccessPermission> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
            => await _accessPermissionBLL.GetByIdAsync(id, cancellationToken);
        

        [HttpPost]
        public async Task<Guid> AddAsync([FromBody] AccessPermissionDetails model, CancellationToken cancellationToken)
            => await _accessPermissionBLL.AddAsync(model, cancellationToken);

        [HttpPut]
        public async Task<Guid> UpdateAsync([FromBody] AccessPermissionDetails model, CancellationToken cancellationToken)
            => await _accessPermissionBLL.UpdateAsync(model, cancellationToken);


        [HttpDelete]
        public async Task RemoveAsync([FromQuery] Guid accessPermissionID, [FromQuery] Guid ownerID, CancellationToken cancellationToken)
            => await _accessPermissionBLL.RemoveAsync(accessPermissionID, ownerID, cancellationToken);
        

        [HttpGet("user/{objectId}")]
        public async Task<AccessPermissionData> GetRightsCurrnetUserAsync([FromRoute] Guid objectId
            , [FromQuery] ObjectClass classID
            , CancellationToken cancellationToken)
            => await _accessPermissionBLL.GetAccessUserToObjectByIDAsync(objectId, classID, cancellationToken);
    }
}
