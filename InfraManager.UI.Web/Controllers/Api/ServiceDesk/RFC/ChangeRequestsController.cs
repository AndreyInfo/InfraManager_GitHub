using AutoMapper;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.ChangeRequests;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.ChangeRequest;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.RFC
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChangeRequestsController : ControllerBase
    {
        private readonly IChangeRequestBLL _changeRequests;
        private readonly IMapper _mapper;
        private readonly IHubContext<EventHub> _hub;

        public ChangeRequestsController(
            IChangeRequestBLL changeRequests,
            IMapper mapper,
            IHubContext<EventHub> hub)
        {
            _changeRequests = changeRequests;
            _mapper = mapper;
            _hub = hub;
        }

        #region REST api
        
        [HttpGet]
        public async Task<ChangeRequestDetailsModel[]> ListAsync([FromQuery] ChangeRequestListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var changeRequests = await _changeRequests.GetDetailsArrayAsync(filterBy, cancellationToken);
            return _mapper.Map<ChangeRequestDetailsModel[]>(changeRequests);
        }

        [HttpGet("{id}")]
        public async Task<ChangeRequestDetailsModel> DetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var details = await _changeRequests.DetailsAsync(id, cancellationToken);
            return _mapper.Map<ChangeRequestDetailsModel>(details);
        }

        [HttpPost]
        public async Task<ChangeRequestDetailsModel> PostAsync(
           [FromBody] ChangeRequestData model,
           CancellationToken cancellationToken = default)
        {
            var details = await _changeRequests.AddAsync(model, cancellationToken);

            EventHub.ObjectInserted(
                _hub,
                (int)ObjectClass.ChangeRequest,
                details.ID,
                null,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<ChangeRequestDetailsModel>(details);
        }

        [HttpPatch("{id}")]
        [HttpPut("{id}")]
        public async Task<ChangeRequestDetailsModel> PatchAsync(
           Guid id,
           [FromBody] ChangeRequestDataModel model,
           CancellationToken cancellationToken = default)
        {
            var data = _mapper.Map<ChangeRequestData>(model);

            var details = await _changeRequests.UpdateAsync(id, data, cancellationToken);

            EventHub.ObjectUpdated(
               _hub,
               (int)ObjectClass.ChangeRequest,
               details.ID,
               null,
               HttpContext.GetRequestConnectionID());

            return _mapper.Map<ChangeRequestDetailsModel>(details);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _changeRequests.DeleteAsync(id, cancellationToken);

            EventHub.ObjectDeleted(
                _hub,
                (int)ObjectClass.ChangeRequest,
                id,
                null,
                HttpContext.GetRequestConnectionID());
        }
        #endregion

        #region Reports

        [HttpGet("reports/allChangeRequests")]
        public async Task<ChangeRequestListItemModel[]> ListAsync([FromQuery] ListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var details = await _changeRequests.GetChangeRequestsAsync(filterBy.ToServiceDeskFilter(), cancellationToken);
            return details.Select(p => _mapper.Map<ChangeRequestListItemModel>(p)).ToArray();
        }

        [HttpGet("reports/availableNotReferencedByMassIncident")]
        public async Task<ChangeRequestReferenceListItemModel[]> AvailableNotReferencedByMassIncidentAsync(
            [FromQuery] InframanagerObjectListViewFilter filterBy,
            CancellationToken cancellationToken = default) =>
            (await _changeRequests.GetAvailableMassIncidentReferencesAsync(filterBy, cancellationToken))
                .Select(item => _mapper.Map<ChangeRequestReferenceListItemModel>(item))
                .ToArray();

        #endregion
    }
}
