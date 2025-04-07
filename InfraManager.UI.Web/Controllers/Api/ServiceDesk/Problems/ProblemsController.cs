using AutoMapper;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Problems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.Web.DTL.SD;
using InfraManager.BLL.ServiceDesk;
using InfraManager.DAL.ServiceDesk;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents;
using Inframanager;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Problems
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProblemsController : ControllerBase
    {
        #region .ctor

        private readonly IProblemBLL _problems;
        private readonly INotesBLL<Problem> _notesBLL;
        private readonly IMapper _mapper;
        private readonly IHubContext<EventHub> _hub;

        public ProblemsController(
            IProblemBLL problems,
            INotesBLL<Problem> notesBLL,
            IMapper mapper,
            IHubContext<EventHub> hub)
        {
            _problems = problems;
            _notesBLL = notesBLL;
            _mapper = mapper;
            _hub = hub;
        }

        #endregion

        #region api/problems

        [HttpGet("reports/allproblems")]
        public async Task<ProblemListItemModel[]> ListAllAsync([FromQuery] ListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var problems = await _problems.AllProblemsArrayAsync(filterBy.ToServiceDeskFilter(), cancellationToken);

            return problems.Select(p => _mapper.Map<ProblemListItemModel>(p)).ToArray();
        }

        [HttpGet]
        public async Task<IEnumerable<ProblemDetailsModel>> ListAsync([FromQuery] ProblemListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var problems = await _problems.GetDetailsArrayAsync(filterBy, cancellationToken);
            return _mapper.Map<IEnumerable<ProblemDetailsModel>>(problems);
        }

        [HttpGet("{id}")]
        public async Task<ProblemDetailsModel> DetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var details = await _problems.DetailsAsync(id, cancellationToken);
            return _mapper.Map<ProblemDetailsModel>(details);
        }

        [HttpPost]
        public async Task<ProblemDetailsModel> PostAsync(
            [FromBody] ProblemData model,
            CancellationToken cancellationToken = default)
        {
            var details = await _problems.AddAsync(model, cancellationToken);

            EventHub.ObjectInserted(
                _hub,
                (int)ObjectClass.Problem,
                details.ID,
                null,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<ProblemDetailsModel>(details);
        }

        [HttpPatch("{id}")]
        public async Task<ProblemDetailsModel> PatchAsync(
            Guid id,
            [FromBody] ProblemData problem,
            CancellationToken cancellationToken = default)
        {
            var details = await _problems.UpdateAsync(
                id,
                problem,
                cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.Problem,
                id,
                null,
                HttpContext.GetRequestConnectionID());
            return _mapper.Map<ProblemDetailsModel>(details);
        }

        [HttpPut("{id}")]
        public async Task<ProblemDetailsModel> PutAsync(
            Guid id,
            [FromBody] ProblemData model,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var details = await _problems.UpdateAsync(id, model, cancellationToken);

                EventHub.ObjectUpdated(
                   _hub,
                   (int)ObjectClass.Problem,
                   details.ID,
                   null,
                   HttpContext.GetRequestConnectionID());
                return _mapper.Map<ProblemDetailsModel>(details);

            }
            catch (NotModifiedException)
            {
                var details = await _problems.DetailsAsync(id, cancellationToken);
                return _mapper.Map<ProblemDetailsModel>(details);
            }
        }

        [HttpPut("{id}/notes/status")]
        public async Task<bool> SetAllNotesReadStatusAsync([FromBody] ObjectListWithNoteStateModel model, CancellationToken cancellationToken = default)
        {
            var problemIDs = model.ObjectList.Where(x => x.ClassID == (int)ObjectClass.Problem).Select(x => x.ID).ToArray();
            await _notesBLL.SetAllNotesReadStateAsync(problemIDs, model.NoteIsReaded, cancellationToken);
            return true;
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _problems.DeleteAsync(id, cancellationToken);

            EventHub.ObjectDeleted(
                _hub,
                (int)ObjectClass.Problem,
                id,
                null,
                HttpContext.GetRequestConnectionID());
        }

        [HttpDelete]
        public async Task DeleteManyAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            foreach (var id in ids)
            {
                await _problems.DeleteAsync(id, cancellationToken).ConfigureAwait(false);
                EventHub.ObjectDeleted(
                    _hub,
                    (int)ObjectClass.Problem,
                    id,
                    null,
                    HttpContext.GetRequestConnectionID());
            }
        }

        #endregion

        #region reports

        [HttpGet("reports/availableNotReferencedByMassIncident")]
        public async Task<ProblemReferenceListItemModel[]> GetAvailableNotReferencedByMassIncidentAsync(
            [FromQuery] InframanagerObjectListViewFilter filterBy,
            CancellationToken cancellationToken = default) =>
                (await _problems.GetAvailableMassIncidentReferencesAsync(filterBy, cancellationToken))
                    .Select(item => _mapper.Map<ProblemReferenceListItemModel>(item))
                    .ToArray();


        #endregion

        #region references

        [HttpPost("{problemID:guid}/ChangeRequests")]
        public async Task<ProblemToChangeRequestReferenceDetails> PostChangeRequestAsync(
            Guid problemID,
            [FromBody] ProblemToChangeRequestReferenceDataModel model,
            CancellationToken cancellationToken = default)
        {
            var details = await _problems.AddChangeRequestAsync(problemID, model.ChangeRequestID, cancellationToken);
            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.Problem,
                details.ProblemID,
                null,
                HttpContext.GetRequestConnectionID());

            return details;
        }

        [HttpDelete("{problemID:guid}/ChangeRequests/{changeRequestID:guid}")]
        public async Task DeleteChangeRequestAsync(Guid problemID, Guid changeRequestID, CancellationToken cancellationToken = default)
        {
            await _problems.RemoveChangeRequestAsync(problemID, changeRequestID, cancellationToken);
            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.Problem,
                problemID,
                null,
                HttpContext.GetRequestConnectionID());
        }

        #endregion

    }
}
