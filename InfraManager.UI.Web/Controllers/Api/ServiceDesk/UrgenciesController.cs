using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.DAL.ServiceDesk;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk
{

    [Route("api/{controller}")]
    [ApiController]
    [Authorize]
    public class UrgenciesController : ControllerBase
    {
        private readonly IUrgencyBLL _urgencyBLL;
        public UrgenciesController(IUrgencyBLL urgencyBLL)
        {
            _urgencyBLL = urgencyBLL;
        }

        [HttpGet]
        public Task<UrgencyListItemModel[]> ListAsync(CancellationToken cancellationToken = default)
        {
            return _urgencyBLL.ListAsync(cancellationToken);
        }

        public sealed class GetModelIn
        {
            public Guid Id { get; set; }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<Urgency> GetByIDAsync([FromQuery] GetModelIn model, CancellationToken cancellationToken = default)
        {
            return await _urgencyBLL.GetAsync(model.Id, cancellationToken);
        }
        
        public sealed class GetListForTableModelIn
        {
            public string SearchRequest { get; set; }
            public int StartRecordIndex { get; set; }
            public int CountRecords { get; set; }
        }

        [HttpPost]
        [Route("Save")]
        public async Task<Guid> SaveAsync([FromBody] UrgencyDTO model, CancellationToken cancellationToken = default)
        {
            return await _urgencyBLL.SaveAsync(model, cancellationToken);
        }

        //[HttpDelete]
        //[Route("Remove")]
        //public ResultData<List<string>> Remove([FromBody] BLL.CrudWeb.DeleteModel<Guid> model)
        //{
        //    var data = _urgencyBLL.Remove(model.ID, model.Name, model.RowVersion);

        //    return data.Success
        //        ? new ResultData<List<string>>(RequestResponceType.Success, data.Result)
        //        : new ResultData<List<string>>((RequestResponceType)(int)data.Fault, null);
        //}
    }
}
