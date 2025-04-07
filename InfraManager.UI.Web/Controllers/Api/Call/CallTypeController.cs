using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.Calls;
using InfraManager.BLL.Catalog;
using InfraManager.BLL.Calls.DTO;
using InfraManager.BLL.CrudWeb;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.UI.Web.Controllers.Api.Call
{
    [Authorize]
    [ApiController]
    [Route("api/call/type")]
    [Obsolete("Use api/calltypes")]
    public class CallTypeController : ControllerBase
    {
        private readonly IBasicCatalogBLL<CallType, CallTypeDetails, Guid, CallType> _callTypeCatalogBLL;
        private readonly ICallTypeBLL _callTypeBLL;

        public CallTypeController(IBasicCatalogBLL<CallType, CallTypeDetails, Guid, CallType> callTypeCatalogBLL,
                                  ICallTypeBLL callTypeBLL)
        {
            _callTypeCatalogBLL = callTypeCatalogBLL;
            _callTypeBLL = callTypeBLL;
        }

        public class GetTreeParent
        {
            public Guid? ParentId { get; set; }
        }

        [HttpPost("tree")]
        public async Task<CallTypeDetails[]> GetTree([FromBody] GetTreeParent model)
        {
            return await _callTypeBLL.GetByParentIDAsync(model.ParentId);
        }

        [HttpGet("list")]
        public async Task<CallTypeDetails[]> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _callTypeBLL.GetListAsync(cancellationToken);
        }

        [HttpGet("item")]
        public async Task<CallTypeDetails> GetByID(Guid id, CancellationToken cancellationToken = default)
        {
            return await _callTypeCatalogBLL.GetByIDAsync(id, cancellationToken);
        }

        [HttpDelete("remove")]
        public async Task<string[]> RemoveAsync([FromBody] DeleteModel<Guid>[] models)
        {
            await _callTypeCatalogBLL.DeleteAsync(models);
            return await _callTypeBLL.DeleteAsync(models);
        }
        
        [HttpGet("path")]
        public async Task<CallTypeDetails[]> GetPathItemById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _callTypeBLL.GetPathItemAsync(id, cancellationToken);
        }
    }
}
