using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalog;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceCatalogue;

internal abstract class ServiceBaseQuery
{
    protected readonly CrossPlatformDbContext _db;
    private readonly IPagingQueryCreator _pagging;
    public ServiceBaseQuery(CrossPlatformDbContext db,
                        IPagingQueryCreator pagging)
    {
        _db = db;
        _pagging = pagging;
    }

    protected async Task<ServiceModelItem[]> GetServiceModelsByQueryAsync(IQueryable<Service> querySerive, PaggingFilter filter, Sort sortProperty, CancellationToken cancellationToken)
    {
        var query = querySerive.Select(c => new ServiceModelItem
        {
            ID = c.ID,
            Name = c.Name,
            State = c.State,
            ServiceCategoryID = c.CategoryID.Value,
            ServiceCategoryName = c.Category.Name,
            Type = c.Type,
            OrganizationItemClassID = c.OrganizationItemClassID,
            OrganizationItemObjectID = c.OrganizationItemObjectID,
            RowVersion = c.RowVersion,
            Note = c.Note,
            CriticalityID = c.CriticalityID,
            ExternalID = c.ExternalID,
            OrganizationItemClassIDCustomer = c.OrganizationItemClassIDCustomer,
            IconName = c.IconName,
            OwnerName = DbFunctions.GetFullObjectName(c.OrganizationItemClassID, c.OrganizationItemObjectID),
            Tags = _db.Set<ServiceTag>().AsNoTracking().Where(v => v.ClassId == ObjectClass.Service && v.ObjectId == c.ID).ToArray(),
            TagNames = string.Join(" ,", _db.Set<ServiceTag>().AsNoTracking().Where(v => v.ClassId == ObjectClass.Service && v.ObjectId == c.ID)
                                                              .Select(c => c.Tag)),
            SupportLineResponsibles = _db.Set<SupportLineResponsible>().Where(v => v.ObjectClassID == ObjectClass.Service && v.ObjectID == c.ID)
                                .Select(c => new SupportLineResponsibleModelItem()
                                {
                                    ObjectID = c.ObjectID,
                                    ObjectClassID = c.ObjectClassID,
                                    ClassID = c.OrganizationItemClassID,
                                    ID = c.OrganizationItemID,
                                    Line = c.LineNumber,
                                    Name = DbFunctions.GetFullObjectName(c.OrganizationItemClassID, c.OrganizationItemID)
                                }).ToArray()
        });

        if (!string.IsNullOrEmpty(filter.SearchString))
            query = query.Where(c => c.Name.ToLower().Contains(filter.SearchString.ToLower())
                                    || c.ServiceCategoryName.ToLower().Contains(filter.SearchString.ToLower())
                                    || c.OwnerName.ToLower().Contains(filter.SearchString.ToLower()));

        var paggingQuery = _pagging.Create(query.OrderBy(sortProperty));

        return await paggingQuery.PageAsync(filter.Skip, filter.Take, cancellationToken);
    }
}