using AutoMapper;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.RFC
{
    [Route("api/ChangeRequests")]
    [ApiController]
    [Authorize]
    public class ChangeRequestNotesController : ControllerBase
    {
        private readonly INotesBLL<ChangeRequest> _notesBLL;
        private readonly IMapper _mapper;
        private readonly IHubContext<EventHub> _hub;

        public ChangeRequestNotesController(INotesBLL<ChangeRequest> notesBLL, IMapper mapper, IHubContext<EventHub> hub)
        {
            _notesBLL = notesBLL;
            _mapper = mapper;
            _hub = hub;
        }

        [HttpGet("{id}/notes")]
        public async Task<NoteListItemModel[]> NotesAsync(Guid id, [FromQuery] bool onlyMessages, CancellationToken cancellationToken = default)
        {
            var notes = await _notesBLL.GetNotesAsync(id, onlyMessages ? DAL.SDNoteType.Message : null, cancellationToken);
            return _mapper.Map<NoteListItemModel[]>(notes);
        }

        [HttpGet("{rfcd}/notes/{id}")]
        public async Task<NoteListItemModel> NoteAsync(Guid rfcd, Guid id, CancellationToken cancellationToken = default)
        {
            var note = await _notesBLL.GetNoteAsync(rfcd, id, cancellationToken);
            return _mapper.Map<NoteListItemModel>(note);
        }

        [HttpPut("{rfcd}/notes/{id}")]
        public async Task<NoteListItemModel> PutAsync(Guid changeRequestId, Guid id, [FromBody] NoteModel noteRequest, CancellationToken cancellationToken = default)
        {
            var note = await _notesBLL.UpdateNotesAsync(new InframanagerObject(changeRequestId, ObjectClass.ChangeRequest), id, _mapper.Map<NoteData>(noteRequest), cancellationToken);
            return _mapper.Map<NoteListItemModel>(note);
        }
        [HttpPost("{id}/notes")]
        public async Task<NoteListItemModel> PostAsync(Guid id, [FromBody] NoteData noteModel, CancellationToken cancellationToken = default)
        {
            var note = await _notesBLL.InsertAsync(new InframanagerObject(id, ObjectClass.ChangeRequest), noteModel, cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.ChangeRequest,
                id,
                null,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<NoteListItemModel>(note);
        }
    }
}
