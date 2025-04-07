using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.DAL.KnowledgeBase;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ServiceDesk.Search
{
    // TODO: Number индексируется сервисом полнотекстового поиска.
    // Тем не менее в легаси поиск по номеру велся в базе.
    // Возможно можно вообще отказаться от такой оптимизации в пользу упрощения логики
    // TODO: Использовать IServiceMapper вместо конкретных BLL
    internal class SearchByNumberStrategy : IServiceDeskSearchStrategy<SearchByNumberParameters>,
        ISelfRegisteredService<IServiceDeskSearchStrategy<SearchByNumberParameters>>
    {
        private readonly IGetEntityArrayBLL<Guid, WorkOrder, WorkOrderDetails, WorkOrderListFilter> _workOrderBll;
        private readonly IGetEntityArrayBLL<Guid, Problem, ProblemDetails, ProblemListFilter> _problemBll;
        private readonly IGetEntityArrayBLL<Guid, Call, CallDetails, CallListFilter> _callBll;
        private readonly IMassIncidentBLL _massiveIncidentBll;
        private readonly IKnowledgeBaseQuery _knowledgeBaseQuery;
        private readonly IMapper _mapper;
        private readonly IValidatePermissions<KBArticle> _kbArticleValidator;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<SearchByNumberStrategy> _logger;
        
        public SearchByNumberStrategy(
            IGetEntityArrayBLL<Guid, WorkOrder, WorkOrderDetails, WorkOrderListFilter> workOrderBll,
            IGetEntityArrayBLL<Guid, Problem, ProblemDetails, ProblemListFilter> problemBll,
            IGetEntityArrayBLL<Guid, Call, CallDetails, CallListFilter> callBll,
            IMassIncidentBLL massiveIncidentBll,
            IKnowledgeBaseQuery knowledgeBaseQuery,
            IMapper mapper,
            IValidatePermissions<KBArticle> kbArticleValidator,
            ICurrentUser currentUser,
            ILogger<SearchByNumberStrategy> logger)
        {
            _workOrderBll = workOrderBll;
            _problemBll = problemBll;
            _callBll = callBll;
            _massiveIncidentBll = massiveIncidentBll;
            _knowledgeBaseQuery = knowledgeBaseQuery;
            _mapper = mapper;
            _kbArticleValidator = kbArticleValidator;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<IReadOnlyList<FoundObject>> SearchAsync(SearchByNumberParameters searchParameters,
            CancellationToken cancellationToken = default)
        {
            var result = new List<FoundObject>(searchParameters.Classes.Count);
            foreach (var el in searchParameters.Classes)
            {
                try
                {
                    switch (el)
                    {
                        case ObjectClass.WorkOrder:
                            var wos = await _workOrderBll.ArrayAsync(new WorkOrderListFilter
                            {
                                Number = searchParameters.Number,
                                ShouldSearchFinished = searchParameters.ShouldSearchFinished
                            }, cancellationToken).ConfigureAwait(false);
                            result.AddRange(wos.Select(_ => _mapper.Map<FoundObject>(_)));
                            break;
                        case ObjectClass.Problem:
                            var problems = await _problemBll.ArrayAsync(new ProblemListFilter
                            {
                                Number = searchParameters.Number,
                                ShouldSearchFinished = searchParameters.ShouldSearchFinished
                            }, cancellationToken).ConfigureAwait(false);
                            result.AddRange(problems.Select(_ => _mapper.Map<FoundObject>(_)));
                            break;
                        case ObjectClass.Call:
                            var calls = await _callBll.ArrayAsync(new CallListFilter
                            {
                                Number = searchParameters.Number,
                                ShouldSearchFinished = searchParameters.ShouldSearchFinished
                            }, cancellationToken).ConfigureAwait(false);
                            result.AddRange(calls.Select(_ => _mapper.Map<FoundObject>(_)));
                            break;
                        case ObjectClass.KBArticle:
                            var article = await _knowledgeBaseQuery
                                .GetArticleByNumberAsync(searchParameters.Number, cancellationToken)
                                .ConfigureAwait(false);
                            if (article != null)
                            {
                                if (!await _kbArticleValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken))
                                    throw new AccessDeniedException($"{nameof(KBArticle)} details array.");
                                
                                result.Add(_mapper.Map<FoundObject>(article));
                            }
                            break;
                        case ObjectClass.MassIncident:
                            try
                            {
                                var massiveIncident = await _massiveIncidentBll
                                    .DetailsAsync(searchParameters.Number, cancellationToken).ConfigureAwait(false);
                                result.Add(_mapper.Map<FoundObject>(massiveIncident));
                            }
                            catch (ObjectNotFoundException)
                            {
                            }

                            break;
                    }
                }
                catch (AccessDeniedException)
                {
                    _logger.LogInformation(
                        "Access to objects of type {Type} not authorized. Not including {Type} to search result.",
                        el.ToString(), el.ToString());
                }
            }

            return result;
        }
    }
}