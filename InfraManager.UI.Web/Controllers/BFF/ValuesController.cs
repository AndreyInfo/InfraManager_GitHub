using InfraManager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.UI.Web.Controllers.BFF
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        private readonly ICurrentUser _currentUser;
        private readonly IListQuery<MassIncident, NegotiationListSubQueryResultItem> _massIncidentQuery;
        private readonly IListQuery<WorkOrder, NegotiationListSubQueryResultItem> _workOrderQuery;
        private readonly IListQuery<Negotiation, NegotiationListSubQueryResultItem, NegotiationListQueryResultItem> _query;

        public ValuesController(
            IListQuery<Negotiation, NegotiationListSubQueryResultItem, NegotiationListQueryResultItem> query,
            IListQuery<MassIncident, NegotiationListSubQueryResultItem> massIncidentQuery,
            IListQuery<WorkOrder, NegotiationListSubQueryResultItem> workOrderQuery,
            ICurrentUser currentUser)
        {
            _query = query;
            _massIncidentQuery = massIncidentQuery;
            _workOrderQuery = workOrderQuery;
            _currentUser = currentUser;
        }

        [HttpGet]
        public NegotiationListQueryResultItem[] Index()        
        {
            var q1 = _massIncidentQuery.Query(
                _currentUser.UserId, 
                Array.Empty<Expression<Func<MassIncident, bool>>>());
            var q2 = _workOrderQuery.Query(
                _currentUser.UserId,
                Array.Empty<Expression<Func<WorkOrder, bool>>>());

            return _query.Query(_currentUser.UserId, q1.Union(q2), Array.Empty<Expression<Func<Negotiation, bool>>>()).ToArray();
        }
    }
}
