using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;

namespace InfraManager.DAL.ServiceCatalog.OperationalLevelAgreements;

public class OperationalLevelAgreementServiceQuery : IOperationalLevelAgreementServiceQuery,
    ISelfRegisteredService<IOperationalLevelAgreementServiceQuery>
{
    private readonly IPagingQueryCreator _queryCreator;
    
    public OperationalLevelAgreementServiceQuery(IPagingQueryCreator queryCreator)
    {
        _queryCreator = queryCreator;
    }

    public Task<OperationalLevelAgreementServiceListItem[]> ExecuteAsync(
        IExecutableQuery<ManyToMany<OperationalLevelAgreement, Service>> query,
        int take,
        int skip,
        bool ascending,
        Expression<Func<OperationalLevelAgreementServiceListItem, object>> predicate,
        string search,
        CancellationToken cancellationToken = default)
    {
        var newQuery = query.Select(x => new OperationalLevelAgreementServiceListItem()
        {
            ID = x.Reference.ID,
            Category = x.Reference.Category.Name,
            Name = x.Reference.Name,
            OwnerName = DbFunctions.GetFullObjectName(x.Reference.OrganizationItemClassID,
                x.Reference.OrganizationItemObjectID),
            State = x.Reference.State
        });

        if (!string.IsNullOrEmpty(search))
        {
            newQuery = newQuery.Where(x => x.Name.ToLower().Contains(search.ToLower()));
        }

        var orderedQuery = ascending ? newQuery.OrderBy(predicate) : newQuery.OrderByDescending(predicate);
        var paggingQuery = _queryCreator.Create(orderedQuery);

        return paggingQuery.PageAsync(skip, take, cancellationToken);
    }
}