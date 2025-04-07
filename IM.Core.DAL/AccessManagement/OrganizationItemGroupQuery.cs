using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.AccessManagement;

internal class OrganizationItemGroupQuery : IOrganizationItemGroupQuery, ISelfRegisteredService<IOrganizationItemGroupQuery>
{
    private readonly DbSet<OrganizationItemGroup> _organizationItemGroups;
    private readonly IPagingQueryCreator _pagging;
    public OrganizationItemGroupQuery(DbSet<OrganizationItemGroup> organizationItemGroups
        , IPagingQueryCreator paging)
    {
        _organizationItemGroups = organizationItemGroups;
        _pagging = paging;
    }
    public async Task<AccessPermissionModelItem[]> ExecuteAsync(Guid objectID
        , ObjectClass objectClass
        , string searchString
        , Sort sortColumn
        , string[] mappedValues
        , int take
        , int skip
        , CancellationToken cancellationToken)
    {
        var query = _organizationItemGroups.AsNoTracking()
                                           .Include(c => c.AccessPermission)
                                           .Where(c => c.AccessPermission != null
                                                      && c.AccessPermission.ObjectID == objectID
                                                      && c.AccessPermission.ObjectClassID == objectClass
                                                      && DbFunctions.GetFullObjectName(c.ItemClassID, c.ItemID) != null)
                                           .Select(c => new AccessPermissionModelItem
                                           {
                                               ID = c.AccessPermission.ID,
                                               ObjectClassID = c.AccessPermission.ObjectClassID,
                                               ObjectID = c.AccessPermission.ObjectID,
                                               OwnerClassID = c.ItemClassID,
                                               OwnerID = c.ItemID,
                                               Name = DbFunctions.GetFullObjectName(c.ItemClassID, c.ItemID),
                                               Properties = c.AccessPermission.Properties,
                                               Add = c.AccessPermission.Add,
                                               Delete = c.AccessPermission.Delete,
                                               Update = c.AccessPermission.Update,
                                               AccessManage = c.AccessPermission.AccessManage,
                                           });

        if (!string.IsNullOrEmpty(searchString))
            query = query.Where(c => EF.Functions.Like(c.Name.ToLower(), searchString.ToLower().ToContainsPattern()));

        var orderedQuery = query.OrderBy(sortColumn);

        for (int i = 1; i < mappedValues.Length; i++)
        {
            var orderedColumn = sortColumn with { PropertyName = mappedValues[i] };
            orderedQuery = orderedQuery.ThenOrderBy(orderedColumn);
        }
        
        var paggingQuery = _pagging.Create(orderedQuery);
        return await paggingQuery.PageAsync(skip, take, cancellationToken);
    }

}
