using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.Core.Logging;

namespace InfraManager.BLL.ServiceDesk.Search
{
    public class SearchNotBoundStrategy : IServiceDeskSearchStrategy<SearchNotBoundParameters>,
        ISelfRegisteredService<IServiceDeskSearchStrategy<SearchNotBoundParameters>>
    {
        private readonly IWorkOrderBLL _workOrderBll;
        private readonly IGetEntityArrayBLL<Guid, Problem, ProblemDetails, ProblemListFilter> _problemBll;
        private readonly IGetEntityArrayBLL<Guid, Call, CallDetails, CallListFilter> _callBll;
        private readonly IServiceMapper<ObjectClass, ICallReferenceBLL> _callReferenceBLL;
        private readonly IMapper _mapper;

        public SearchNotBoundStrategy(
            IWorkOrderBLL workOrderBll,
            IGetEntityArrayBLL<Guid, Problem, ProblemDetails, ProblemListFilter> problemBll,
            IGetEntityArrayBLL<Guid, Call, CallDetails, CallListFilter> callBll,
            IServiceMapper<ObjectClass, ICallReferenceBLL> callReferenceBLL,
            IMapper mapper)
        {
            _workOrderBll = workOrderBll;
            _problemBll = problemBll;
            _callBll = callBll;
            _callReferenceBLL = callReferenceBLL;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<FoundObject>> SearchAsync(SearchNotBoundParameters searchParameters,
            CancellationToken cancellationToken = default)
        {
            var result = new List<FoundObject>(searchParameters.Classes.Count);
            foreach (var el in searchParameters.Classes)
            {
                try
                {
                    switch (el)
                    {
                        case ObjectClass.Problem:
                        {
                            var problems = await _problemBll.ArrayAsync(new ProblemListFilter(), cancellationToken);
                            var callReference =
                                (await _callReferenceBLL
                                    .Map(ObjectClass.Problem)
                                    .GetAsync(new CallReferenceListFilter { ObjectID = searchParameters.ParentID },
                                        cancellationToken))
                                .Select(c => c.ObjectID)
                                .ToHashSet();

                            result.AddRange(problems.Where(p => !callReference.Contains(p.ID))
                                .Select(_ => _mapper.Map<FoundObject>(_)));
                            break;
                        }

                        case ObjectClass.WorkOrder:
                        {
                            var workOrders = await _workOrderBll.GetDetailsArrayAsync(new WorkOrderListFilter());

                            // if not 0 - it is already linked to some entity
                            var notLinkedWorkOrders = workOrders.Where(wo => wo.WorkOrderReferenceID == 0);

                            result.AddRange(_mapper.Map<FoundObject[]>(notLinkedWorkOrders));
                            break;
                        }

                        case ObjectClass.Call:
                        {
                            var calls = await _callBll.ArrayAsync(new CallListFilter(), cancellationToken);
                            var callReference =
                                (await _callReferenceBLL.Map(ObjectClass.ChangeRequest).GetAsync(
                                    new CallReferenceListFilter { CallID = searchParameters.ParentID },
                                    cancellationToken))
                                .Union(await _callReferenceBLL.Map(ObjectClass.Problem)
                                    .GetAsync(new CallReferenceListFilter { CallID = searchParameters.ParentID },
                                        cancellationToken))
                                .Select(c => c.CallID).ToHashSet();
                            result.AddRange(calls.Where(c => !callReference.Contains(c.ID))
                                .Select(_ => _mapper.Map<FoundObject>(_)));
                            break;
                        }
                    }
                }
                catch (AccessDeniedException)
                {
                    Logger.Error($"Access to objects of type {el.ToString()} not authorized. Not including {el.ToString()}s to search result.");
                }
            }

            return result;
        }
    }
}