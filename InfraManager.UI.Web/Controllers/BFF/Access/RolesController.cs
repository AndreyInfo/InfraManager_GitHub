using InfraManager.BLL.Catalog;
using InfraManager.BLL.Roles;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.AccessManagement;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.AccessManagement.ForTable;
using Microsoft.AspNetCore.Authorization;
using InfraManager.Core.Extensions;
using System.Linq;
using Inframanager.BLL;
using InfraManager.BLL.AccessManagement.Roles;

namespace InfraManager.UI.Web.Controllers.BFF.Access
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IBasicCatalogBLL<Role, RoleListItemDetails, Guid, RoleForTable> _basicCatalogBLL;
        private readonly IRolesBLL _rolesBLL;
        public RolesController(IBasicCatalogBLL<Role, RoleListItemDetails, Guid, RoleForTable> basicCatalogBLL, IRolesBLL rolesBLL)
        {
            _basicCatalogBLL = basicCatalogBLL;
            _rolesBLL = rolesBLL;
        }

        [HttpGet("list")]
        public async Task<RoleDetails[]> GetDetailsListAsync([FromQuery] RoleFilter filterBy,
            [FromQuery] ClientPageFilter<Role> pageBy, CancellationToken cancellationToken = default)
            => string.IsNullOrWhiteSpace(pageBy.OrderByProperty)
                ? await _rolesBLL.GetDetailsArrayAsync(filterBy, cancellationToken)
                : await _rolesBLL.GetDetailsPageAsync(filterBy, pageBy, cancellationToken);


        [HttpGet] 
        public async Task<RoleListItemDetails[]> GetTableAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken)
        {
            return await _basicCatalogBLL.GetByFilterAsync(filter, cancellationToken);
            
        }

        [HttpGet("{id}")]
        public async Task<RoleDetails> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return await _rolesBLL.GetAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<RoleDetails> AddAsync([FromBody] RoleInsertDetails model, CancellationToken cancellationToken)
        {
            return await _rolesBLL.AddAsync(model, cancellationToken);
        } 

        [HttpPut("{roleID}")]
        public async Task<RoleDetails> UpdateAsync([FromRoute] Guid roleID, [FromBody] RoleData model, CancellationToken cancellationToken)
        {
            return await _rolesBLL.UpdateAsync(roleID, model, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _rolesBLL.DeleteAsync(id, cancellationToken);
        }

        [HttpGet("User/{userID}")]
        public async Task<UserRolesWithSelectedDetails[]> GetUserRolesAsync([FromRoute] Guid userID,
            CancellationToken cancellationToken)
        {
            return await _rolesBLL.GetUserRolesAsync(userID, cancellationToken);
        }

        [HttpPut("User/{userID}")]
        public async Task UpdateUserRolesAsync([FromRoute] Guid userID,
            [FromBody] UserRolesWithSelectedData[] userRoles, CancellationToken cancellationToken)
        {
            await _rolesBLL.SetRoleForUserAsync(userRoles, userID, cancellationToken);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<RoleDetails[]> GetByUserAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
        {
            return await _rolesBLL.GetByUserAsync(userId, cancellationToken);
        }
    }
}
