using AutoMapper;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Calls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Calls
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CallsController : ControllerBase
    {
        private readonly ICallBLL _service;
        private readonly IMapper _mapper;
        private readonly IHubContext<EventHub> _hub;
        private readonly IUserFieldsToDictionaryResolver _userFieldsToDictionaryResolver;

        public CallsController(
            ICallBLL service,
            IMapper mapper,
            IHubContext<EventHub> hub,
            IUserFieldsToDictionaryResolver userFieldsToDictionaryResolver)
        {
            _service = service;
            _mapper = mapper;
            _hub = hub;
            _userFieldsToDictionaryResolver = userFieldsToDictionaryResolver;
        }

        #region api/calls

        [HttpPost]
        public async Task<ActionResult<CallDetailsModel>> AddCallAsync([FromBody] CallData model, CancellationToken cancellationToken = default)
        {
            var details = await _service.AddAsync(model, cancellationToken);

            var detailsModel = _mapper.Map<CallDetailsModel>(details);
            EventHub.ObjectInserted(    
                _hub,
                (int)ObjectClass.Call,
                detailsModel.ID,
                null,
                HttpContext.GetRequestConnectionID());

            return detailsModel;
        }

        [HttpPatch("{id}")]
        [HttpPut("{id}")] //todo: его надо убрать
        public async Task<ActionResult<CallDetailsModel>> PutAsync(Guid id, [FromBody] CallData model,
            CancellationToken cancellationToken = default)
        {
            var details = await _service.UpdateAsync(id, model, cancellationToken);
            var detailsModel = _mapper.Map<CallDetailsModel>(details);
            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.Call,
                detailsModel.ID,
                null,
                HttpContext.GetRequestConnectionID());

            return detailsModel;
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _service.DeleteAsync(id, cancellationToken);
            EventHub.ObjectDeleted(
                _hub,
                (int)ObjectClass.Call,
                id,
                null,
                HttpContext.GetRequestConnectionID());
        }

        [HttpGet("{id}")]
        public async Task<CallDetailsModel> FindAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var callDetails = await _service.DetailsAsync(id, cancellationToken);
            callDetails.UserFieldNamesDictionary = _userFieldsToDictionaryResolver.Resolve(callDetails);
            return _mapper.Map<CallDetailsModel>(callDetails);
        }

        [HttpGet]
        public async Task<IEnumerable<CallDetailsModel>> ListAsync([FromQuery] CallListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var calls = await _service.GetDetailsArrayAsync(filterBy, cancellationToken);
            return calls.Select(c => _mapper.Map<CallDetailsModel>(c));
        }
        #endregion

        #region reports

        [HttpPost("reports/allCalls")]
        public async Task<CallListItemModel[]> AllCallsAsync(ListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var items = await _service.AllCallsAsync(
                filterBy.ToServiceDeskFilter(),
                cancellationToken);

            return items.Select(x => _mapper.Map<CallListItemModel>(x)).ToArray();
        }

        [HttpGet("reports/callsFromMe")]
        public async Task<CallsFromMeListItemModel[]> CallsFromMeListAsync([FromQuery] ListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var callsFromMe = await _service.CallsFromMeAsync(
                filterBy.ToCallFromMeListFilter(),
                cancellationToken);

            return callsFromMe.Select(x => _mapper.Map<CallsFromMeListItemModel>(x)).ToArray();
        }

        [HttpGet("reports/availableNotReferencedByMassIncident")]
        public async Task<CallReferenceListItemModel[]> CallsAvailableNotReferencedByMassIncidentAsync(
            [FromQuery] InframanagerObjectListViewFilter filterBy,
            CancellationToken cancellationToken = default) =>
                (await _service.GetAvailableMassIncidentReferencesAsync(filterBy, cancellationToken))
                    .Select(item => _mapper.Map<CallReferenceListItemModel>(item))
                    .ToArray();


        #endregion
    }
}
