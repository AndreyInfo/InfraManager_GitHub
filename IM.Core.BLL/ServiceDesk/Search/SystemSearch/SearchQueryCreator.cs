using System;
using System.Linq;
using System.Linq.Expressions;
using Inframanager.BLL.ListView;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Search.SystemSearch;

public class SearchQueryCreator<TEntity> : ISearchQueryCreator
{
    private readonly IListViewUserSpecification<TEntity, MyTasksReportItem> _userSpecificationBuilder;
    private readonly IListQuery<TEntity, MyTasksListQueryResultItem> _query;

    public SearchQueryCreator(IListViewUserSpecification<TEntity, MyTasksReportItem> userSpecificationBuilder,
        IListQuery<TEntity, MyTasksListQueryResultItem> query)
    {
        _userSpecificationBuilder = userSpecificationBuilder;
        _query = query;
    }

    public IQueryable<MyTasksListQueryResultItem> CreateQuery(Guid id)
    {
        return _query.Query(id, new Expression<Func<TEntity, bool>>[] { _userSpecificationBuilder.Build(id) });
    }
}