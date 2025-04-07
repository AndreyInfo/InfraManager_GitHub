using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using InfraManager.BLL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public class WorkOrderDetailsBuilder : IBuildObject<WorkOrderDetails, WorkOrder>,
        ISelfRegisteredService<IBuildObject<WorkOrderDetails, WorkOrder>>
    {
        private readonly IMapper _mapper;
        private readonly IReadNegotiationBLL _readNegotiationBLL;

        public WorkOrderDetailsBuilder(IMapper mapper, IReadNegotiationBLL readNegotiationBLL)
        {
            _mapper = mapper;
            _readNegotiationBLL = readNegotiationBLL;
        }

        public async Task<WorkOrderDetails> BuildAsync(WorkOrder data, CancellationToken cancellationToken = default)
        {
            var details = _mapper.Map<WorkOrderDetails>(data);
            var filterBy = new NegotiationListFilter
            {
                Parent = new InframanagerObject(data.IMObjID, ObjectClass.WorkOrder)
            };

            var negotiations = await _readNegotiationBLL.GetDetailsArrayAsync(filterBy, cancellationToken);

            details.NegotiationCount = negotiations.Count();

            return details;

        }

        public async Task<IEnumerable<WorkOrderDetails>> BuildManyAsync(IEnumerable<WorkOrder> dataItems, CancellationToken cancellationToken = default)
        {
            List<WorkOrderDetails> woDetails = new();
            foreach (var item in dataItems) 
                woDetails.Add(await BuildAsync(item, cancellationToken));

            return woDetails;
        }
    }
}
