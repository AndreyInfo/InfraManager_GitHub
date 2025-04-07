using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using CallEntity = InfraManager.DAL.ServiceDesk.Call;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.SignalR;
using InfraManager.Web.DTL.SD;
using System.Linq;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Calls
{
    [Route("api/calls")]
    [ApiController]
    [Authorize]
    public class CallNotesController : ControllerBase
    {
        #region .ctor

        private readonly INotesBLL<CallEntity> _notesBLL;
        private readonly IMapper _mapper;
        private readonly IHubContext<EventHub> _hub;

        public CallNotesController(INotesBLL<CallEntity> notesBLL, IMapper mapper, IHubContext<EventHub> hub)
        {
            _notesBLL = notesBLL;
            _mapper = mapper;
            _hub = hub;
        }

        #endregion

        #region api/calls

        [HttpGet("{id}/notes")]
        public async Task<NoteListItemModel[]> NotesAsync(Guid id, [FromQuery] bool onlyMessages,
            CancellationToken cancellationToken = default)
        {
            var notes = await _notesBLL.GetNotesAsync(id, onlyMessages ? DAL.SDNoteType.Message : null,
                cancellationToken);
            return _mapper.Map<NoteListItemModel[]>(notes);
        }

        [HttpGet("{callId}/notes/{id}")]
        public async Task<NoteListItemModel> NoteAsync(Guid callId, Guid id,
            CancellationToken cancellationToken = default)
        {
            var note = await _notesBLL.GetNoteAsync(callId, id, cancellationToken);
            return _mapper.Map<NoteListItemModel>(note);
        }

        [HttpPut("{callId}/notes/status")]
        public async Task<bool> SetAllNotesReadStatusAsync([FromBody] ObjectListWithNoteStateModel model, CancellationToken cancellationToken = default)
        {
            var callIDs = model.ObjectList.Where(x => x.ClassID == (int)ObjectClass.Call).Select(x => x.ID).ToArray();
            await _notesBLL.SetAllNotesReadStateAsync(callIDs, model.NoteIsReaded, cancellationToken);
            return true;
        }

        [HttpPut("{callId}/notes/{id}")]
        public async Task<NoteListItemModel> PutAsync(Guid callId, Guid id, [FromBody] NoteModel noteRequest,
            CancellationToken cancellationToken = default)
        {
            var note = await _notesBLL.UpdateNotesAsync(new InframanagerObject(callId, ObjectClass.Call), id, _mapper.Map<NoteData>(noteRequest), cancellationToken);
            return _mapper.Map<NoteListItemModel>(note);
        }

        [HttpPost("{callId}/notes")]
        public async Task<NoteListItemModel> PostAsync(Guid callId, [FromBody] NoteData noteModel,
            CancellationToken cancellationToken = default)
        {
            var note = await _notesBLL.InsertAsync(new InframanagerObject(callId, ObjectClass.Call), noteModel, cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.Call,
                callId,
                null,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<NoteListItemModel>(note);
        }
        #endregion
    }
}