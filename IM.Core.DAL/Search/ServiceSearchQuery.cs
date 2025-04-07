using System;
using System.Linq;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Search;

internal class ServiceSearchQuery : IServiceSearchQuery, ISelfRegisteredService<IServiceSearchQuery>
{
    private readonly DbContext _db;

    public ServiceSearchQuery(CrossPlatformDbContext db)
    {
        _db = db;
    }

    public IQueryable<ObjectSearchResult> Query(ServiceSearchCriteria criteria, Guid userID)
    {
        var query = _db.Set<Service>()
            .Include(x => x.Category)
            .AsNoTracking();

        if (criteria.Types != null && criteria.Types.Any())
        {
            query = query.Where(s => criteria.Types.Contains(s.Type));
        }

        if (criteria.States != null && criteria.States.Any())
        {
            query = query.Where(s => criteria.States.Contains(s.State));
        }

        if (criteria.ExceptServiceIDs != null 
            && criteria.ExceptServiceIDs.Any())
        {
            query = query.Where(s => !criteria.ExceptServiceIDs.Contains(s.ID));
        }

        if (!string.IsNullOrWhiteSpace(criteria.Text))
        {
            var pattern = criteria.Text.ToLower();
            query = query.Where(x => (x.Category.Name + " \\ " + x.Name).ToLower().Contains(pattern));
        }

        return query.Select(x => new ObjectSearchResult
        {
            ID = x.ID,
            ClassID = ObjectClass.Service,
            FullName = x.Category.Name + " \\ " + x.Name,
        });
    }
}