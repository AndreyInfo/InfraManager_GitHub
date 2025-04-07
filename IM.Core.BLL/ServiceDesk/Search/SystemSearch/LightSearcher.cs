using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.AccessManagement;
using Inframanager.BLL.ListView;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Search.SystemSearch
{
    public class LightSearcher : ILightSearcher, ISelfRegisteredService<ILightSearcher>
    {
        private readonly IPagingQueryCreator _queryCreator;
        private readonly IServiceMapper<ObjectClass, ISearchQueryCreator> _queryCreators;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly IUserAccessBLL _validatePermissions;
    
        private readonly Dictionary<ObjectClass, OperationID> _neededOperations = new()
        {
            { ObjectClass.Call, OperationID.Call_Properties },
            { ObjectClass.WorkOrder, OperationID.WorkOrderPriority_Properties },
            { ObjectClass.Problem, OperationID.Problem_Properties },
            { ObjectClass.MassIncident, OperationID.MassIncident_Properties }
        };

        public LightSearcher(IPagingQueryCreator queryCreator,
            IServiceMapper<ObjectClass, ISearchQueryCreator> queryCreators,
            ICurrentUser currentUser,
            IMapper mapper,
            IUserAccessBLL validatePermissions)
        {
            _queryCreator = queryCreator;
            _queryCreators = queryCreators;
            _currentUser = currentUser;
            _mapper = mapper;
            _validatePermissions = validatePermissions;
        }

        public async Task<MyTasksReportItem[]> SearchAsync(SearchFilter filter, CancellationToken cancellationToken = default)
        {
            IQueryable<MyTasksListQueryResultItem> query = null;
        
            foreach (var el in filter.Classes)
            {
                if (await _validatePermissions.UserHasOperationAsync(_currentUser.UserId, _neededOperations[el],
                        cancellationToken) && _queryCreators.HasKey(el))
                {
                    var createdQuery = _queryCreators.Map(el).CreateQuery(_currentUser.UserId);
                    query = query == null ? createdQuery : query.Union(createdQuery);
                }
            }

            if (query == null)
            {
                return Array.Empty<MyTasksReportItem>();
            }

            var orderedQuery = BuildFilterQuery(query, filter);

            var result = await orderedQuery.PageAsync(filter.Skip, filter.Take, cancellationToken);
            return _mapper.Map<MyTasksReportItem[]>(result);
        }

        private IPagingQuery<MyTasksListQueryResultItem> BuildFilterQuery(IQueryable<MyTasksListQueryResultItem> query,
            SearchFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Text) && int.TryParse(filter.Text, out int objectNumber))
            {
                query = query.Where(x => x.Number == objectNumber);
            }
        
            if (!filter.SearchFinished)
            {
                query = query.Where(x => !x.IsFinished);
            }
        
            if (filter.IDs != null)
            {
                query = query.Where(x => filter.IDs.Contains(x.ID));
            }

            var orderedQuery = _queryCreator.Create(query.OrderBy(new Sort
            {
                Ascending = filter.Ascending,
                PropertyName = filter.OrderBy
            }));

            return orderedQuery;
        }
    }
}