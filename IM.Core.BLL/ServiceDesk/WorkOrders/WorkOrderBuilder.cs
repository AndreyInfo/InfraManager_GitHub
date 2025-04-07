using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.BLL.Settings;
using InfraManager.BLL.Workflow;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.CustomControl;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class WorkOrderBuilder :
        IBuildObject<WorkOrder, WorkOrderData>,
        ISelfRegisteredService<IBuildObject<WorkOrder, WorkOrderData>>
    {
        private readonly IMapper _mapper;
        private readonly IReadonlyRepository<WorkOrderPriority> _priorityRepository;
        private readonly IProvideWorkOrderReference _referenceProvider;
        private readonly ISettingsBLL _settingsBLL;
        private readonly IConvertSettingValue<long> _valueConverter;
        private readonly ICalendarServiceBLL _calendarServiceBLL;
        private readonly ICreateWorkflow<WorkOrder> _workflow;
        private readonly IServiceMapper<ObjectClass, IModifyWorkOrderExecutorControl> _underControlModifierMapper;

        public WorkOrderBuilder(
            IMapper mapper,
            IReadonlyRepository<WorkOrderPriority> priorityRepository,
            IProvideWorkOrderReference referenceProvider,
            ISettingsBLL settingsBLL,
            IConvertSettingValue<long> valueConverter,
            ICalendarServiceBLL calendarServiceBLL,
            ICreateWorkflow<WorkOrder> workflow,
            IServiceMapper<ObjectClass, IModifyWorkOrderExecutorControl> underControlModifierMapper)
        {
            _mapper = mapper;
            _priorityRepository = priorityRepository;
            _referenceProvider = referenceProvider;
            _settingsBLL = settingsBLL;
            _valueConverter = valueConverter;
            _calendarServiceBLL = calendarServiceBLL;
            _workflow = workflow;
            _underControlModifierMapper = underControlModifierMapper;
        }

        public async Task<WorkOrder> BuildAsync(WorkOrderData data, CancellationToken cancellationToken = default)
        {
            var workOrder = _mapper.Map<WorkOrder>(data);

            if (!data.PriorityID.HasValue)
            {
                workOrder.PriorityID = await _priorityRepository.GetDefaultPriorityIDAsync()
                    ?? throw new InvalidObjectException("Priority is missing"); //TODO локализация
            }

            long valueToAdd;
            var now = DateTime.UtcNow;
            if (data.DatePromisedDeltaInMinutes.HasValue)
            {
                valueToAdd = TimeSpan.FromMinutes(data.DatePromisedDeltaInMinutes.Value).Ticks;
            }
            else
            {
                var datePromisedTimeSpan = await _settingsBLL.GetValueAsync(SystemSettings.WorkOrderFinishDateDelta, cancellationToken);
                valueToAdd = _valueConverter.Convert(datePromisedTimeSpan);
            }

            workOrder.UtcDatePromised = await _calendarServiceBLL.GetUtcFinishDateByCalendarAsync(DateTime.UtcNow, new TimeSpan(valueToAdd), null, null);

            if (data.DateStartedDeltaInMinutes.HasValue)
            {
                workOrder.UtcDateStarted = now.AddMinutes(data.DateStartedDeltaInMinutes.Value);
            }

            workOrder.WorkOrderReference = await _referenceProvider.GetOrCreateAsync(data.ReferencedObject, cancellationToken);
            if (workOrder.ExecutorID.HasValue && !workOrder.WorkOrderReference.IsDefault && _underControlModifierMapper.HasKey(workOrder.WorkOrderReference.ObjectClassID))
            {
                await _underControlModifierMapper
                    .Map(workOrder.WorkOrderReference.ObjectClassID)
                    .SetUnderControlIfNeededAsync(workOrder.WorkOrderReference.ObjectID, workOrder.ExecutorID.Value, cancellationToken);
            }

            await _workflow.TryStartNewAsync(workOrder, cancellationToken);

            return workOrder;
        }

        public Task<IEnumerable<WorkOrder>> BuildManyAsync(IEnumerable<WorkOrderData> dataItems, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
