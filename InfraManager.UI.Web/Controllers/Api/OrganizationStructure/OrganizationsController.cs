using InfraManager.BLL.OrganizationStructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.UI.Web.Controllers.Api.OrganizationStructure
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationBLL _service;

        public OrganizationsController(IOrganizationBLL service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<OrganizationDetails[]> GetAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken= default)
        {
            return filter.IsEmpty()
                ? await _service.GetAllAsync(cancellationToken)
                : await _service.GetListAsync(filter, cancellationToken); //TODO: переделать этот метод в BLL
        }

        [HttpGet("{id}")]
        public Task<OrganizationDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _service.DetailsAsync(id, cancellationToken);
        }
        
        [HttpPost]
        public async Task<Guid> AddOrganizationAsync([FromBody] OrganizationData organization, CancellationToken cancellationToken) => 
            await _service.AddOrganizationAsync(organization, cancellationToken);
        

        [HttpPut("{id}")]
        public async Task UpdateOrganizationAsync([FromRoute] Guid id, [FromBody] OrganizationData organization,
            CancellationToken cancellationToken) =>
            await _service.UpdateOrganizationAsync(id,organization,cancellationToken);

        [HttpDelete("{id}")]
        public async Task DeleteOrganizationAsync([FromRoute] Guid id, CancellationToken cancellationToken) =>
            await _service.DeleteByIdAsync(id, cancellationToken);

        [HttpGet]
        [Route("reports/allOrganizations")]
        [Obsolete("Метод добавлен для обеспечения работы дерева оргструктуры и совместимости с админкой. Удалить/изменить после обновления админки.")]
        public Task<OrganizationDetails[]> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _service.GetAllAsync(cancellationToken);
        }
    }
}
