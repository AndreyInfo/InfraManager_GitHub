using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.WorkOrders
{
    [Route("api/workorders")]
    [ApiController]
    [Authorize]
    public class WorkOrderNotesController : ControllerBase
    {
        private readonly INotesBLL<WorkOrder> _notesBLL;
        private readonly IMapper _mapper;
        private readonly IHubContext<EventHub> _hub;

        public WorkOrderNotesController(INotesBLL<WorkOrder> notesBLL, IMapper mapper, IHubContext<EventHub> hub)
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

        [HttpGet("{workorderId}/notes/{id}")]
        public async Task<NoteListItemModel> NoteAsync(Guid workorderId, Guid id, CancellationToken cancellationToken = default)
        {
            var note = await _notesBLL.GetNoteAsync(workorderId, id, cancellationToken);
            return _mapper.Map<NoteListItemModel>(note);
        }

        [HttpPut("{workorderId}/notes/{id}")]
        public async Task<NoteListItemModel> PutAsync(Guid workorderId, Guid id, [FromBody] NoteModel noteRequest, CancellationToken cancellationToken = default)
        {
            var note = await _notesBLL.UpdateNotesAsync(new InframanagerObject(workorderId, ObjectClass.WorkOrder), id, _mapper.Map<NoteData>(noteRequest), cancellationToken);
            return _mapper.Map<NoteListItemModel>(note);
        }
        [HttpPost("{workorderId}/notes")]
        public async Task<NoteListItemModel> PostAsync(Guid workorderId, [FromBody] NoteData noteModel, CancellationToken cancellationToken = default)
        {
            var note = await _notesBLL.InsertAsync(new InframanagerObject(workorderId, ObjectClass.WorkOrder), noteModel, cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.WorkOrder,
                workorderId,
                null,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<NoteListItemModel>(note);
        }
    }
}
