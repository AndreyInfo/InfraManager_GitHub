using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.DAL.ServiceDesk;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Problems
{
    [Route("api/Problems")]
    [ApiController]
    [Authorize]
    public class ProblemNotesController : ControllerBase
    {
        private readonly INotesBLL<Problem> _notesBLL;
        private readonly IMapper _mapper;
        private readonly IHubContext<EventHub> _hub;

        public ProblemNotesController(INotesBLL<Problem> notesBLL, IMapper mapper, IHubContext<EventHub> hub)
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

        [HttpGet("{problemId}/notes/{id}")]
        public async Task<NoteListItemModel> NoteAsync(Guid problemId, Guid id, CancellationToken cancellationToken = default)
        {
            var note = await _notesBLL.GetNoteAsync(problemId, id, cancellationToken);
            return _mapper.Map<NoteListItemModel>(note);
        }

        [HttpPut("{problemId}/notes/{id}")]
        public async Task<NoteListItemModel> PutAsync(Guid problemId, Guid id, [FromBody] NoteModel noteRequest, CancellationToken cancellationToken = default)
        {
            var note = await _notesBLL.UpdateNotesAsync(new InframanagerObject(problemId, ObjectClass.Problem), id, _mapper.Map<NoteData>(noteRequest), cancellationToken);
            return _mapper.Map<NoteListItemModel>(note);
        }
        [HttpPost("{problemId}/notes")]
        public async Task<NoteListItemModel> PostAsync(Guid problemId, [FromBody] NoteData noteModel, CancellationToken cancellationToken = default)
        {
            var note = await _notesBLL.InsertAsync(new InframanagerObject(problemId, ObjectClass.Problem), noteModel, cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.Problem,
                problemId,
                null,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<NoteListItemModel>(note);
        }
    }
}
