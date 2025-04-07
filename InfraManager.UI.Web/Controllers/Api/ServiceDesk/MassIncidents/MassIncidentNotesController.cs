using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.MassIncidents
{
    // TODO: исправить копипасту CallNotesController и прочих
    [Route("api/massIncidents")]
    [ApiController]
    [Authorize]
    public class MassIncidentNotesController : ControllerBase
    {
        private readonly IMassIncidentBLL _massIncidentsBll;
        private readonly INotesBLL<MassIncident> _notesBLL;
        private readonly IMapper _mapper;
        private readonly IHubContext<EventHub> _hub;

        public MassIncidentNotesController(
            IMassIncidentBLL massIncidentsBll,
            INotesBLL<MassIncident> notesBLL, 
            IMapper mapper, 
            IHubContext<EventHub> hub)
        {
            _massIncidentsBll = massIncidentsBll;
            _notesBLL = notesBLL;
            _mapper = mapper;
            _hub = hub;
        }

        [HttpGet("{id}/notes")]
        public async Task<NoteListItemModel[]> NotesAsync(int id, [FromQuery] bool onlyMessages, CancellationToken cancellationToken = default)
        {
            var massIncident = await _massIncidentsBll.DetailsAsync(id, cancellationToken);
            var notes = await _notesBLL.GetNotesAsync(massIncident.IMObjID, onlyMessages ? DAL.SDNoteType.Message : null, cancellationToken);
            return _mapper.Map<NoteListItemModel[]>(notes);
        }

        [HttpGet("{massIncidentID}/notes/{id}")]
        public async Task<NoteListItemModel> NoteAsync(int massIncidentID, Guid id, CancellationToken cancellationToken = default)
        {
            var massIncident = await _massIncidentsBll.DetailsAsync(massIncidentID, cancellationToken);
            var note = await _notesBLL.GetNoteAsync(massIncident.IMObjID, id, cancellationToken);
            return _mapper.Map<NoteListItemModel>(note);
        }

        [HttpPut("{massIncidentID}/notes/{id}")]
        public async Task<NoteListItemModel> PutAsync(int massIncidentID, Guid id, [FromBody] NoteModel noteRequest, CancellationToken cancellationToken = default)
        {
            var massIncident = await _massIncidentsBll.DetailsAsync(massIncidentID, cancellationToken);
            var note = await _notesBLL.UpdateNotesAsync(new InframanagerObject(massIncident.IMObjID, ObjectClass.MassIncident), id, _mapper.Map<NoteData>(noteRequest), cancellationToken);
            return _mapper.Map<NoteListItemModel>(note);
        }
        [HttpPost("{massIncidentID}/notes")]
        public async Task<NoteListItemModel> PostAsync(int massIncidentID, [FromBody] NoteData noteModel, CancellationToken cancellationToken = default)
        {
            var massIncident = await _massIncidentsBll.DetailsAsync(massIncidentID, cancellationToken);
            var note = await _notesBLL.InsertAsync(new InframanagerObject(massIncident.IMObjID, ObjectClass.WorkOrder), noteModel, cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.MassIncident,
                massIncident.IMObjID,
                null,
                HttpContext.GetRequestConnectionID());
            EventHub.ObjectInserted(
                _hub,
                (int)ObjectClass.Notification,
                note.ID,
                massIncident.IMObjID,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<NoteListItemModel>(note);
        }
    }
}
