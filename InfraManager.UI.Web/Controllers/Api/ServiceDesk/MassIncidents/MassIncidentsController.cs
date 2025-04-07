using AutoMapper;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL.ListView;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Calls;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.WorkOrders;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.MassIncidents
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MassIncidentsController : ControllerBase
    {
        #region .ctor

        private readonly IMassIncidentBLL _service;
        private readonly IMapper _mapper;
        private readonly IHubContext<EventHub> _hub;

        public MassIncidentsController(IMassIncidentBLL service, IMapper mapper, IHubContext<EventHub> hub)
        {
            _service = service;
            _mapper = mapper;
            _hub = hub;
        }

        #endregion

        #region crud

        [HttpGet]
        public async Task<MassIncidentDetailsModel[]> GetAsync([FromQuery] MassIncidentListFilter filterBy, CancellationToken cancellationToken = default) =>
            (await _service.GetDetailsArrayAsync(filterBy, cancellationToken)).Select(x=>Model(x)).ToArray();

        [HttpGet("{id}")]
        public async Task<MassIncidentDetailsModel> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var details = await _service.DetailsAsync(id, cancellationToken);

            return Model(details);
        }

        [HttpPost]
        public async Task<MassIncidentDetailsModel> PostAsync([FromBody]NewMassIncidentData data, CancellationToken cancellationToken = default)
        {
            var details = await _service.AddAsync(data, cancellationToken);
            EventHub.ObjectInserted(
                _hub,
                (int)ObjectClass.MassIncident,
                details.IMObjID,
                null,
                HttpContext.GetRequestConnectionID());

            return Model(details);
        }

        [HttpPatch("{id}")]
        [HttpPut("{id}")] //todo: нужно убрать
        public async Task<MassIncidentDetailsModel> PatchAsync(int id, [FromBody]MassIncidentData data, CancellationToken cancellationToken = default)
        {
            var details = await _service.UpdateAsync(id, data, cancellationToken);
            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.MassIncident,
                details.IMObjID,
                null,
                HttpContext.GetRequestConnectionID());

            return Model(details);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var details = await _service.DetailsAsync(id, cancellationToken);
            await _service.DeleteAsync(id, cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.MassIncident,
                details.IMObjID,
                null,
                HttpContext.GetRequestConnectionID());
        }

        private MassIncidentDetailsModel Model(MassIncidentDetails details)
        {
            return _mapper.Map<MassIncidentDetailsModel>(details);
        }

        #endregion

        #region references

        private async Task<MassIncidentReferenceDetails> PostReferenceAsync(Func<Task<MassIncidentReferenceDetails>> action)
        {
            var details = await action();
            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.MassIncident,
                details.MassIncidentID,
                null,
                HttpContext.GetRequestConnectionID());

            return details;
        }

        private async Task DeleteReferenceAsync(int id, Func<Task> deleteAction, CancellationToken cancellationToken = default)
        {
            var details = await _service.DetailsAsync(id, cancellationToken);
            await deleteAction();
            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.MassIncident,
                details.IMObjID,
                null,
                HttpContext.GetRequestConnectionID());
        }

        [HttpGet("{id}/calls")]
        public Task<MassIncidentReferenceDetails[]> GetCallsAsync(int id, CancellationToken cancellationToken = default) =>
            _service.GetCallsAsync(id, cancellationToken);

        [HttpPost("{id}/calls")]
        public Task<MassIncidentReferenceDetails> PostCallAsync(int id, [FromBody]MassIncidentReferenceDataModel model, CancellationToken cancellationToken = default)
        {
            return PostReferenceAsync(() => _service.AddCallAsync(id, model.ReferenceID, cancellationToken));
        }

        [HttpDelete("{id}/calls/{callID}")]
        public Task DeleteCallAsync(int id, Guid callID, CancellationToken cancellationToken = default)
        {
            return DeleteReferenceAsync(id, () => _service.RemoveCallAsync(id, callID, cancellationToken), cancellationToken);
        }

        [HttpGet("{id}/problems")]
        public async Task<MassIncidentReferenceDetails[]> GetProblemsAsync(int id, CancellationToken cancellationToken = default)
            => await _service.GetProblemsAsync(id, cancellationToken);

        [HttpPost("{id}/problems")]
        public Task<MassIncidentReferenceDetails> PostProblemAsync(int id, [FromBody] MassIncidentReferenceDataModel model, CancellationToken cancellationToken = default)
        {
            return PostReferenceAsync(() => _service.AddProblemAsync(id, model.ReferenceID, cancellationToken));
        }

        [HttpDelete("{id}/problems/{problemID}")]
        public Task DeleteProblemAsync(int id, Guid problemID, CancellationToken cancellationToken = default)
        {
            return DeleteReferenceAsync(id, () => _service.RemoveProblemAsync(id, problemID, cancellationToken), cancellationToken);
        }

        [HttpGet("{id}/changeRequests")]
        public Task<MassIncidentReferenceDetails[]> GetChangeRequestAsync(int id, CancellationToken cancellationToken = default) =>
            _service.GetChangeRequestAsync(id, cancellationToken);

        [HttpPost("{id}/changeRequests")]
        public Task<MassIncidentReferenceDetails> PostChangeRequestAsync(int id, [FromBody] MassIncidentReferenceDataModel model, CancellationToken cancellationToken = default)
        {
            return PostReferenceAsync(() => _service.AddChangeRequestAsync(id, model.ReferenceID, cancellationToken));
        }

        [HttpDelete("{id}/changeRequests/{changeRequestID}")]
        public Task DeleteChangeRequestAsync(int id, Guid changeRequestID, CancellationToken cancellationToken = default)
        {
            return DeleteReferenceAsync(id, () => _service.RemoveChangeRequestAsync(id, changeRequestID, cancellationToken), cancellationToken);
        }

        [HttpGet("{id}/affectedServices")]
        public async Task<ServiceReferenceModel[]> GetAffectedServicesAsync(int id, CancellationToken cancellationToken = default)
        {
            var details = await _service.GetAffectedServicesAsync(id, cancellationToken);

            return details.Select(x => _mapper.Map<ServiceReferenceModel>(x)).ToArray();
        }

        [HttpPost("{id}/affectedServices")]
        public async Task<ServiceReferenceModel> PostServiceAsync(int id, [FromBody] MassIncidentReferenceDataModel model, CancellationToken cancellationToken = default)
        {
            var details = await _service.AddAffectedServiceAsync(id, model.ReferenceID, cancellationToken);
            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.MassIncident,
                details.MassIncidentID,
                null,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<ServiceReferenceModel>(details);
        }

        [HttpDelete("{id}/affectedServices/{serviceItemOrAttendanceID}")]
        public Task DeleteServiceAsync(int id, Guid serviceItemOrAttendanceID, CancellationToken cancellationToken = default)
        {
            return DeleteReferenceAsync(id, () => _service.RemoveAffectedServiceAsync(id, serviceItemOrAttendanceID, cancellationToken), cancellationToken);
        }

        #endregion

        #region reports

        [HttpPost("reports/allMassIncidents")]
        public async Task<AllMassIncidentsReportItemModel[]> AllCallsAsync(ListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var items = await _service.AllMassIncidentsAsync(
                filterBy.ToServiceDeskFilter(),
                cancellationToken);

            return items.Select(x => _mapper.Map<AllMassIncidentsReportItemModel>(x)).ToArray();
        }

        [HttpGet("{id}/reports/referencedCalls")]
        public async Task<CallReferenceListItemModel[]> ReferencedCallsReportAsync(int id, [FromQuery] MassIncidentReferencesListViewFilter pageFilter, CancellationToken cancellationToken = default)
        {
            var listItems = await _service.GetReferencedCallsReportAsync(id, pageFilter, cancellationToken);
            return listItems.Select(item => _mapper.Map<CallReferenceListItemModel>(item)).ToArray();
        }

        [HttpGet("{id}/reports/referencedChangeRequests")]
        public async Task<ChangeRequestReferenceListItemModel[]> ReferencedChangeRequestsReportAsync(int id, [FromQuery] MassIncidentReferencesListViewFilter pageFilter, CancellationToken cancellationToken = default)
        {
            var listItems = await _service.GetReferencedChangeRequestsReportAsync(id, pageFilter, cancellationToken);
            return listItems.Select(item => _mapper.Map<ChangeRequestReferenceListItemModel>(item)).ToArray();
        }

        [HttpGet("{id}/reports/referencedProblems")]
        public async Task<ProblemReferenceListItemModel[]> ReferencedProblemsReportAsync(int id, [FromQuery] MassIncidentReferencesListViewFilter pageFilter, CancellationToken cancellationToken = default)
        {
            var listItems = await _service.GetReferencedProblemsReportAsync(id, pageFilter, cancellationToken);
            return listItems.Select(item => _mapper.Map<ProblemReferenceListItemModel>(item)).ToArray();
        }

        [HttpGet("{id}/reports/referencedWorkOrders")]
        public async Task<WorkOrderReferenceListItemModel[]> ReferencedWorkOrdersReportAsync(int id, [FromQuery] MassIncidentReferencesListViewFilter pageFilter, CancellationToken cancellationToken = default)
        {
            var listItems = await _service.GetReferencedWorkOrdersReportAsync(id, pageFilter, cancellationToken);
            return listItems.Select(item => _mapper.Map<WorkOrderReferenceListItemModel>(item)).ToArray();
        }

        [HttpPost("reports/MassIncidentsToAssociate")]
        public async Task<MassIncidentsReportItemModel[]> GetMassIncidentsToAssociateAsync(MassIncidentsToAssociateFilter filterBy, CancellationToken cancellationToken = default)
        {
            var items = await _service.GetMassIncidentsToAssociateAsync(
                new ListViewFilterData<MassIncidentsToAssociateFilter>
                {
                    ExtensionFilter = filterBy,
                    ViewName = filterBy.ViewName,
                    Take = filterBy.CountRecords,
                    Skip = filterBy.StartRecordIndex,
                    CustomFilters = filterBy.CustomFilters,
                    StandardFilter = filterBy.StandardFilter,
                    CurrentFilterID = filterBy.CurrentFilterID,
                },
                cancellationToken);
            return _mapper.Map<MassIncidentsReportItemModel[]>(items);
        }

        [HttpPost("reports/ProblemMassIncidents")]
        public async Task<MassIncidentsReportItemModel[]> GetProblemMassIncidentsAsync(ProblemMassIncidentFilter filterBy, CancellationToken cancellationToken = default)
        {
            var items = await _service.GetProblemMassIncidentsAsync(
                new ListViewFilterData<ProblemMassIncidentFilter>
                {
                    ExtensionFilter = filterBy,
                    ViewName = filterBy.ViewName,
                    Take = filterBy.CountRecords,
                    Skip = filterBy.StartRecordIndex,
                    CustomFilters = filterBy.CustomFilters,
                    StandardFilter = filterBy.StandardFilter,
                    CurrentFilterID = filterBy.CurrentFilterID
                },
                cancellationToken);
            return _mapper.Map<MassIncidentsReportItemModel[]>(items);
        }

        #endregion
    }
}
