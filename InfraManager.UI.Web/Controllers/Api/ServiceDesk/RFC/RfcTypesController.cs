using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk.ChangeRequests;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.RFC
{
    [Route("api/{controller}")]
    [ApiController]
    [Authorize]
    public class RfcTypesController : ControllerBase
    {
        private readonly IRfcTypeBLL _service; 

        public RfcTypesController(IRfcTypeBLL service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<RfcTypeDetailsModel[]> ListAsync([FromQuery] LookupListFilter filter, CancellationToken cancellationToken = default)
        {
            return await _service.GetDetailsArrayAsync(filter, cancellationToken);
        }

        [HttpGet("{id}")]
        public Task<RfcTypeDetailsModel> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            return _service.DetailsAsync(id, cancellationToken);
        }
        
        [HttpPost]
        public async Task<RfcTypeDetailsModel> AddAsync(
            [FromBody] RfcTypeModel newRfcType,
            CancellationToken cancellationToken = default)
        {
            return await _service.AddAsync(newRfcType, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<RfcTypeDetailsModel> PutAsync(
            Guid id,
            [FromBody] RfcTypeModel newRfcType,
            CancellationToken cancellationToken = default)
        {
            return await _service.UpdateAsync(id, newRfcType, cancellationToken);
        }


        [HttpDelete("{id}")]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            await _service.DeleteAsync(id, cancellationToken);
        } 
        
        
    }
}
