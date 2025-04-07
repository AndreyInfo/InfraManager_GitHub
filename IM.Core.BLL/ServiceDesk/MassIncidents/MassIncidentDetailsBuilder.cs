using InfraManager.BLL.ServiceDesk.WorkOrders;
using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System.Threading;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    public class MassIncidentDetailsBuilder : IBuildObject<MassIncidentDetails, MassIncident>,
        ISelfRegisteredService<IBuildObject<MassIncidentDetails, MassIncident>>
    {
        private readonly IMapper _mapper;
        private readonly IReadonlyRepository<WorkOrder> _workOrders;

        public MassIncidentDetailsBuilder(IMapper mapper, IReadonlyRepository<WorkOrder> workOrders)
        {
            _mapper = mapper;
            _workOrders = workOrders;
        }

        public async Task<MassIncidentDetails> BuildAsync(MassIncident data, CancellationToken cancellationToken = default)
        {
            var details = _mapper.Map<MassIncidentDetails>(data);
            details.WorkOrderCount = (await _workOrders.With(x=>x.WorkOrderReference).Query(x=>x.WorkOrderReference.ObjectID == data.IMObjID && x.WorkOrderReference.ObjectClassID == ObjectClass.MassIncident).ExecuteAsync(cancellationToken)).Count();
            return details;
        }

        public async Task<IEnumerable<MassIncidentDetails>> BuildManyAsync(IEnumerable<MassIncident> dataItems, CancellationToken cancellationToken = default)
        {
            var details = dataItems.Select(async x => await BuildAsync(x, cancellationToken));
            await Task.WhenAll(details);
            return details.Select(x => x.Result);
        }
    }
}
