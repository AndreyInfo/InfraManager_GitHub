using AutoMapper;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.Negotiations;
using InfraManager.DAL.ServiceDesk.Negotiations;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Negotiations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Negotiations
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NegotiationsController : ControllerBase
    {
        private readonly IServiceMapper<ObjectClass, IEditNegotiationBLL> _serviceMapper;
        private readonly IReadNegotiationBLL _readNegotiationBLL;
        private readonly IMapper _mapper;
        private readonly IHubContext<EventHub> _hub;

        public NegotiationsController(
            IServiceMapper<ObjectClass, IEditNegotiationBLL> serviceMapper, 
            IReadNegotiationBLL readNegotiationBLL,
            IMapper mapper,
            IHubContext<EventHub> hub)
        {
            _serviceMapper = serviceMapper;
            _readNegotiationBLL = readNegotiationBLL;
            _mapper = mapper;
            _hub = hub;
        }

        [HttpGet("reports/myNegotiations")]
        public async Task<ListItemModel[]> ListAsync(
            [FromQuery] ListFilter filterBy,
            [FromQuery] ObjectClass? objectClassId,
            [FromQuery] Guid? objectId,
            CancellationToken cancellationToken = default)
        {
            var negotiations = await _readNegotiationBLL.GetReportAsync(
                new ListViewFilterData<NegotiationListFilter>
                {
                    ExtensionFilter = new NegotiationListFilter
                    {
                        WithFinishedWorkflow = filterBy.WithFinishedWorkflow,
                        AfterModifiedMilliseconds = filterBy.AfterModifiedMilliseconds,
                        IDList = filterBy.IDList,
                        Parent = objectId.HasValue && objectClassId.HasValue
                            ? new InframanagerObject(objectId.Value, objectClassId.Value)
                            : null
                    },
                    CurrentFilterID = filterBy.CurrentFilterID,
                    CustomFilters = filterBy.CustomFilters,
                    StandardFilter = filterBy.StandardFilter,
                    ViewName = filterBy.ViewName,
                    Take = filterBy.CountRecords,
                    Skip = filterBy.StartRecordIndex
                }
                , cancellationToken);

            return negotiations
                .Select(x => _mapper.Map<NegotiationListItem, NegotiationReportItemModel>(x))
                .ToArray();
        }

        [HttpGet]
        public async Task<NegotiationDetailsModel[]> GetAsync(
            [FromQuery] ObjectClass? objectClassId,
            [FromQuery] Guid? objectId,
            [FromQuery] Guid? userID,
            [FromQuery] ClientPageFilter<Negotiation> pageFilter,
            CancellationToken cancellationToken = default)
        {
            var filterBy = new NegotiationListFilter
            {
                Parent = objectId.HasValue
                        ? new InframanagerObject(objectId.Value, objectClassId.Value)
                        : null,
                UserID = userID
            };

            var details = string.IsNullOrWhiteSpace(pageFilter.OrderByProperty)
                ? await _readNegotiationBLL.GetDetailsArrayAsync(filterBy, cancellationToken)
                : await _readNegotiationBLL.GetDetailsPageAsync(filterBy, pageFilter, cancellationToken);

            return details.Select(x => _mapper.Map<NegotiationDetailsModel>(x)).ToArray();
        }

        [HttpGet("{id}")]
        public async Task<NegotiationDetailsModel> DetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var details = await _readNegotiationBLL.DetailsAsync(id, cancellationToken);

            return _mapper.Map<NegotiationDetailsModel>(details);
        }

        [HttpPost("/api/{classID}/{id}/negotiations")]
        public async Task<NegotiationDetailsModel> PostAsync(
            ObjectClass classID,
            Guid id,
            [FromBody]NegotiationData negotiation,
            CancellationToken cancellationToken = default)
        {
            var details = await _serviceMapper
                .Map(classID)
                .AddAsync(id, negotiation, cancellationToken);

            EventHub.ObjectInserted(
                _hub,
                (int)ObjectClass.Negotiation,
                details.ID,
                id,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<NegotiationDetailsModel>(details);
        }

        [HttpPatch("{id}")]
        public async Task<NegotiationDetailsModel> PatchAsync(
            Guid id,
            [FromBody] NegotiationData negotiation,
            CancellationToken cancellationToken = default)
        {
            var details = await _readNegotiationBLL.DetailsAsync(id, cancellationToken);
            details = await _serviceMapper
                .Map(details.ObjectClassID)
                .UpdateAsync(id, negotiation, cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.Negotiation,
                id,
                details.ObjectID,
                HttpContext.GetRequestConnectionID());
            return _mapper.Map<NegotiationDetailsModel>(details);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var details = await _readNegotiationBLL.DetailsAsync(id, cancellationToken);
            await _serviceMapper.Map(details.ObjectClassID).DeleteAsync(id, cancellationToken);

            EventHub.ObjectDeleted(
                _hub,
                (int)ObjectClass.Negotiation,
                details.ID,
                details.ObjectID,
                HttpContext.GetRequestConnectionID());
        }

        [HttpDelete("{id}/users/{userId}")]
        public async Task DeleteUserAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var details = await _readNegotiationBLL.DetailsAsync(id, cancellationToken);           
            await _serviceMapper.Map(details.ObjectClassID).DeleteNegotiationUserAsync(id, userId, cancellationToken);
        }

        [HttpPatch("{id}/users/{userId}")]
        public async Task<NegotiationUserDetailsModel> PatchNegotiationUserAsync(Guid id, Guid userId, [FromBody]VoteData data, CancellationToken cancellationToken = default)
        {
            var details = await _readNegotiationBLL.DetailsAsync(id, cancellationToken);
            var userDetails = await _serviceMapper.Map(details.ObjectClassID).UpdateNegotiationUserAsync(id, userId, data, cancellationToken);

            return _mapper.Map<NegotiationUserDetailsModel>(userDetails);
        }
    }
}
