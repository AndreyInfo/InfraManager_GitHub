using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.Negotiations;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallDetailsBuilder : 
        IBuildObject<CallDetails, Call>,
        ISelfRegisteredService<IBuildObject<CallDetails, Call>>
    {
        private readonly IMapper _mapper;
        private readonly IServiceMapper<ObjectClass, IFindNameByGlobalID> _locationNameFinder;
        private readonly IServiceMapper<ObjectClass, ICallReferenceBLL> _refBLL;
        private readonly IReadNegotiationBLL _readNegotiationBLL;

        public CallDetailsBuilder(
            IMapper mapper, 
            IServiceMapper<ObjectClass, IFindNameByGlobalID> locationNameFinder,
            IServiceMapper<ObjectClass, ICallReferenceBLL> refBLL,
            IReadNegotiationBLL readNegotiationBLL)
        {
            _mapper = mapper;
            _locationNameFinder = locationNameFinder;
            _refBLL = refBLL;
            _readNegotiationBLL = readNegotiationBLL;
        }

        public async Task<CallDetails> BuildAsync(Call data, CancellationToken cancellationToken = default)
        {
            var details = _mapper.Map<CallDetails>(data);
            details.ProblemsRefCount = (await _refBLL.Map(ObjectClass.Problem).GetAsync(new CallReferenceListFilter() { CallID = data.IMObjID }, cancellationToken)).Count();

            details.NegotiationCount = (await _readNegotiationBLL.GetDetailsArrayAsync(new NegotiationListFilter { Parent = new InframanagerObject(data.IMObjID, ObjectClass.Call) }, cancellationToken)).Count();


            if (data.ServicePlaceID.HasValue && data.ServicePlaceClassID.HasValue)
            {
                var servicePlaceName = await _locationNameFinder
                    .Map(data.ServicePlaceClassID.Value)
                    .FindAsync(data.ServicePlaceID.Value, cancellationToken);
                details.ServicePlaceName = servicePlaceName;
                details.ServicePlaceNameShort = servicePlaceName;
            }

            return details;
        }

        public async Task<IEnumerable<CallDetails>> BuildManyAsync(IEnumerable<Call> dataItems, CancellationToken cancellationToken = default)
        {
            var details = new List<CallDetails>();
            foreach(var item in dataItems)
            {
                details.Add(await BuildAsync(item, cancellationToken));
            }

            return details.AsReadOnly();
        }
    }
}
