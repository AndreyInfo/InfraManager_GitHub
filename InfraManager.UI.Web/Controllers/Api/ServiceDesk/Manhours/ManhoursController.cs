using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.ServiceDesk.Manhours;
using InfraManager.BLL.Settings;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Manhours;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Manhours
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManhoursController : ControllerBase
    {
        private readonly IManhoursWorkBLL _service;
        private readonly ISettingsBLL _settings;
        private readonly IMapper _mapper;
        private readonly IHubContext<EventHub> _hub;

        public ManhoursController(
            IManhoursWorkBLL service,
            ISettingsBLL settings,
            IMapper mapper,
            IHubContext<EventHub> hub)
        {
            _service = service;
            _settings = settings;
            _mapper = mapper;
            _hub = hub;
        }

        [HttpGet("[action]")]
        public async Task<ManhoursSettingsModel> SettingsAsync(CancellationToken cancellationToken = default)
        {
            var showFullInterval = (bool)await _settings
                .ConvertValueAsync(SystemSettings.ManHoursValueType, cancellationToken);
            var showInClosed = (bool)await _settings
                .ConvertValueAsync(SystemSettings.ManHoursInClosed, cancellationToken);
            return new ManhoursSettingsModel
            {
                AllowInClosed = showInClosed,
                ShowFullInterval = showFullInterval
            };
        }

        [HttpGet]
        public async Task<ManhoursWorkDetailsModel[]> GetAsync(
            [FromQuery] ObjectClass objectClassId,
            [FromQuery] Guid objectId,
            CancellationToken cancellationToken = default)
        {
            var details = await _service.GetWorkDetailsArrayAsync(
                new ManhoursListFilter
                {
                    Parent = new InframanagerObject(objectId, objectClassId)
                }, cancellationToken);

            return details.Select(d => _mapper.Map<ManhoursWorkDetailsModel>(d)).ToArray();
        }

        [HttpGet("{id}")]
        public async Task<ManhoursWorkDetailsModel> GetAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var details = await _service.GetWorkDetailsAsync(id, cancellationToken);
            return _mapper.Map<ManhoursWorkDetailsModel>(details);
        }

        [HttpDelete("{workID}")]
        public async Task DeleteAsync(Guid workID, CancellationToken cancellationToken = default)
        {
            var details = await _service.GetWorkDetailsAsync(workID, cancellationToken);
            await _service.DeleteWorkAsync(workID, cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)details.ObjectClassID,
                details.ObjectID,
                null,
                HttpContext.GetRequestConnectionID());

            EventHub.ObjectDeleted(
                _hub,
                (int)ObjectClass.ManhoursWork,
                workID,
                details.ObjectID,
                HttpContext.GetRequestConnectionID());
        }

        [HttpPost("/api/calls/{id}/manhours")]
        public Task<ManhoursWorkDetailsModel> AddCallManhoursWorkAsync(
            Guid id,
            [FromForm] ManhoursWorkDataModel data,
            CancellationToken cancellationToken = default)
        {
            return AddManhoursWorkAsync(ObjectClass.Call, id, data, cancellationToken);
        }

        [HttpPost("/api/problems/{id}/manhours")]
        public Task<ManhoursWorkDetailsModel> AddProblemManhoursWorkAsync(
            Guid id,
            [FromForm] ManhoursWorkDataModel data,
            CancellationToken cancellationToken = default)
        {
            return AddManhoursWorkAsync(ObjectClass.Problem, id, data, cancellationToken);
        }
        [HttpPost("/api/RFC/{id}/manhours")]
        public Task<ManhoursWorkDetailsModel> AddRFCManhoursWorkAsync(
            Guid id,
            [FromForm] ManhoursWorkDataModel data,
            CancellationToken cancellationToken = default)
        {
            return AddManhoursWorkAsync(ObjectClass.ChangeRequest, id, data, cancellationToken);
        }

        [HttpPost("/api/workorders/{id}/manhours")]
        public Task<ManhoursWorkDetailsModel> AddWorkOrderManhoursWorkAsync(
            Guid id,
            [FromForm] ManhoursWorkDataModel data,
            CancellationToken cancellationToken = default)
        {
            return AddManhoursWorkAsync(ObjectClass.WorkOrder, id, data, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<ManhoursWorkDetailsModel> PutAsync(
            Guid id,
            [FromForm] ManhoursWorkDataModel data,
            CancellationToken cancellationToken = default)
        {
            var details = await _service.UpdateWorkAsync(
                id,
                _mapper.Map<ManhoursWorkData>(data),
                cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)details.ObjectClassID,
                details.ObjectID,
                null,
                HttpContext.GetRequestConnectionID());
            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.ManhoursWork,
                id,
                null,
                HttpContext.GetRequestConnectionID());
            return _mapper.Map<ManhoursWorkDetailsModel>(details);
        }

        [HttpDelete("{workID}/{id}")]
        public async Task<ManhoursWorkDetailsModel> DeleteAsync(
            Guid workID,
            Guid id,
            CancellationToken cancellationToken = default)
        {
            await _service.DeleteManhourEntryAsync(workID, id, cancellationToken);
            var details = await _service.GetWorkDetailsAsync(workID, cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)details.ObjectClassID,
                details.ObjectID,
                null,
                HttpContext.GetRequestConnectionID());

            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.ManhoursWork,
                details.ID,
                details.ObjectID,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<ManhoursWorkDetailsModel>(details);
        }

        [HttpPost("/api/manhours/{workID}/manhour")]
        public async Task<ManhoursWorkDetailsModel> PostAsync(Guid workID, [FromForm] ManhourData data,
            CancellationToken cancellationToken = default)
        {
            var details = await _service.AddManhourEntryAsync(workID, data, cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)details.ObjectClassID,
                details.ObjectID,
                null,
                HttpContext.GetRequestConnectionID());

            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.ManhoursWork,
                details.ID,
                details.ObjectID,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<ManhoursWorkDetailsModel>(details);
        }

        [HttpPut("/api/manhours/{workID}/manhour")]
        public async Task<ManhoursWorkDetailsModel> PutAsync(Guid workID, [FromForm] ManhourData data,
            CancellationToken cancellationToken = default)
        {
            var details = await _service.UpdateManhourEntryAsync(workID, data, cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)details.ObjectClassID,
                details.ObjectID,
                null,
                HttpContext.GetRequestConnectionID());

            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.ManhoursWork,
                details.ID,
                details.ID,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<ManhoursWorkDetailsModel>(details);
        }

        private async Task<ManhoursWorkDetailsModel> AddManhoursWorkAsync(
            ObjectClass classID,
            Guid id,
            ManhoursWorkDataModel data,
            CancellationToken cancellationToken = default)
        {
            var details = await _service.AddWorkAsync(
                new InframanagerObject(id, classID),
                _mapper.Map<ManhoursWorkData>(data, opts => opts.AfterMap((_,d) =>
                {
                    d.ObjectID = id;
                    d.ObjectClassID = classID;
                })),
                cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)classID,
                id,
                null,
                HttpContext.GetRequestConnectionID());

            EventHub.ObjectInserted(
                _hub,
                (int)ObjectClass.ManhoursWork,
                details.ID,
                id,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<ManhoursWorkDetailsModel>(details);
        }
    }
}