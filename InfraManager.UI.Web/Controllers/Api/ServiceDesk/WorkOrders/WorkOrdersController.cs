using AutoMapper;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.WorkOrders;
using InfraManager.BLL.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.Web.DTL.SD;
using InfraManager.DAL.ServiceDesk;
using System.Collections.Generic;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.WorkOrders
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkOrdersController : ControllerBase
    {
        #region .ctor

        private readonly IWorkOrderBLL _workOrders;
        private readonly INotesBLL<WorkOrder> _notesBLL;
        private readonly IMapper _mapper;
        private readonly IHubContext<EventHub> _hub;
        private readonly IUserFieldsToDictionaryResolver _userFieldsToDictionaryResolver;

        public WorkOrdersController(
            IWorkOrderBLL workOrders,
            INotesBLL<WorkOrder> notesBLL,
            IMapper mapper, 
            IHubContext<EventHub> hub,
            IUserFieldsToDictionaryResolver userFieldsToDictionaryResolver)
        {
            _workOrders = workOrders;
            _notesBLL = notesBLL;
            _mapper = mapper;
            _hub = hub;
            _userFieldsToDictionaryResolver = userFieldsToDictionaryResolver;
        }

        #endregion

        #region api/workorders

        [HttpGet]
        public async Task<WorkOrderDetailsModel[]> GetAsync([FromQuery] WorkOrderListFilter filter, CancellationToken cancellationToken = default)
        {
            var data = await _workOrders.GetDetailsPageAsync(filter, cancellationToken);

            return data.Select(x => _mapper.Map<WorkOrderDetailsModel>(x)).ToArray();
        }

        [HttpGet("{id}")]
        public async Task<WorkOrderDetailsModel> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var details = await _workOrders.DetailsAsync(id, cancellationToken);
            details.UserFieldNamesDictionary = _userFieldsToDictionaryResolver.Resolve(details);
            return _mapper.Map<WorkOrderDetailsModel>(details);
        }

        [HttpPost]
        public async Task<WorkOrderDetailsModel> PostAsync(
            [FromBody] WorkOrderDataModel model,
            CancellationToken cancellationToken = default)
        {
            var data = _mapper.Map<WorkOrderData>(model);
            var details = await _workOrders.AddAsync(data, cancellationToken);

            EventHub.ObjectInserted(
                _hub,
                (int)ObjectClass.WorkOrder,
                details.ID,
                null,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<WorkOrderDetailsModel>(details);
        }
        
        [HttpPatch("{id}")]
        [HttpPut("{id}")] //todo: нужно убрать
        public async Task<WorkOrderDetailsModel> PutAsync(
            Guid id,
            [FromBody] WorkOrderDataModel model,
            CancellationToken cancellationToken = default)
        {
            var data = _mapper.Map<WorkOrderData>(model);
            var details = await _workOrders.UpdateAsync(id, data, cancellationToken);

            EventHub.ObjectUpdated(
                _hub,
                (int)ObjectClass.WorkOrder,
                details.ID,
                null,
                HttpContext.GetRequestConnectionID());

            return _mapper.Map<WorkOrderDetailsModel>(details);
        }

        [HttpPut("{id}/notes/status")]
        public async Task<bool> SetAllNotesReadStatusAsync([FromBody] ObjectListWithNoteStateModel model, CancellationToken cancellationToken = default)
        {
            var workOrderIDs = model.ObjectList.Where(x => x.ClassID == (int)ObjectClass.WorkOrder).Select(x => x.ID).ToArray();
            await _notesBLL.SetAllNotesReadStateAsync(workOrderIDs, model.NoteIsReaded, cancellationToken);
            return true;
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _workOrders.DeleteAsync(id, cancellationToken);

            EventHub.ObjectDeleted(
                _hub,
                (int)ObjectClass.WorkOrder,
                id,
                null,
                HttpContext.GetRequestConnectionID());
        }

        [HttpDelete]
        public async Task DeleteManyAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            foreach (var id in ids)
            {
                await _workOrders.DeleteAsync(id, cancellationToken);

                EventHub.ObjectDeleted(
                    _hub,
                    (int)ObjectClass.WorkOrder,
                    id,
                    null,
                    HttpContext.GetRequestConnectionID());
            }
        }
        #endregion

        #region api/workorders/reports

        [HttpGet("reports/allWorkOrders")]
        public async Task<WorkOrderListItemModel[]> ListAsync([FromQuery] ListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var workOrders = await _workOrders.GetAllWorkOrdersAsync(filterBy.ToServiceDeskFilter(), cancellationToken);
            return workOrders
                .Select(workOrder => _mapper.Map<WorkOrderListItemModel>(workOrder))
                .ToArray();
        }

        [HttpPost("reports/inventoryWorkOrders")]
        public async Task<InventoryListItemModel[]> GetInventoryReportAsync(ListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var inventories = await _workOrders.GetInventoryReportAsync(filterBy.ToServiceDeskFilter(), cancellationToken);
            return _mapper.Map<InventoryListItemModel[]>(inventories);
        }
        #endregion

        /// <summary>
        /// Получить список связей Задания асинхронно.
        /// </summary>
        /// <param name="id">Уникальный идентификатор Задания.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
        /// <returns>Список связанных с заданием объектов.</returns>
        [HttpGet("{id:guid}/references")]
        public async Task<InframanagerObject[]> GetReferencesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var references = await _workOrders.GetReferencesAsync(id, cancellationToken);
            return _mapper.Map<InframanagerObject[]>(references);
        }
        
        [HttpGet("references")]
        public async Task<WorkOrderListItemModel[]> ListAsync([FromQuery] WorkOrderListFilter filterBy,
            [FromQuery] BaseFilter baseFilter, CancellationToken cancellationToken = default)
        {
            var result = await _workOrders.Get(filterBy.ToWorkOrderServiceDeskFilter(baseFilter), cancellationToken);   
            return result
                .Select(workOrder => _mapper.Map<WorkOrderListItemModel>(workOrder))
                .ToArray();        
        }

        [HttpGet("reports/availableNotReferencedByMassIncident")]
        public async Task<WorkOrderReferenceListItemModel[]> CallsAvailableNotReferencedByMassIncidentAsync(
            [FromQuery] InframanagerObjectListViewFilter filterBy,
            CancellationToken cancellationToken = default) =>
                (await _workOrders.GetAvailableMassIncidentReferencesAsync(filterBy, cancellationToken))
                    .Select(item => _mapper.Map<WorkOrderReferenceListItemModel>(item))
                    .ToArray();
    }
}
