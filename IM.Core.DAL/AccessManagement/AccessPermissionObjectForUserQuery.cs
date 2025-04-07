using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.AccessManagement;
internal sealed class AccessPermissionObjectForUserQuery :
    IAccessPermissionObjectForUserQuery
    , ISelfRegisteredService<IAccessPermissionObjectForUserQuery>
{
    private readonly DbSet<User> _users;
    private readonly DbSet<GroupUser> _groupUsers;
    private readonly DbSet<OrganizationItemGroup> _organizationItemGroups;

    public AccessPermissionObjectForUserQuery(DbSet<User> users
        , DbSet<GroupUser> groupUsers
        , DbSet<OrganizationItemGroup> organizationItemGroups)
    {
        _users = users;
        _groupUsers = groupUsers;
        _organizationItemGroups = organizationItemGroups;
    }

    public async Task<AccessPermissionObject[]> ExecuteAsync(Guid userID, ObjectClass? objectClassID, CancellationToken cancellationToken)
    {
        var divisions = await GetSubdivisionsByUserAsync(userID, cancellationToken);
        var query = _organizationItemGroups.AsNoTracking();

        if(objectClassID.HasValue)
            query = query.Where(c=> c.AccessPermission.ObjectClassID == objectClassID);

        return await query.Select(c => new AccessPermissionObject
        {
            OwnerID = c.ItemID,
            OwnerClassID = c.ItemClassID,
            ObjectID = c.AccessPermission.ObjectID,
            ObjectClassID = c.AccessPermission.ObjectClassID,
        })
        .Where(c => (c.OwnerClassID == ObjectClass.User && userID == c.OwnerID)
                    
                    || (c.OwnerClassID == ObjectClass.Group 
                            && _groupUsers.Any(v => v.GroupID == c.OwnerID && v.UserID == userID))
                    
                    || (c.OwnerClassID == ObjectClass.Organizaton 
                            && _users.Any(v => v.IMObjID == userID && v.Subdivision.OrganizationID == c.OwnerID))
                    
                    || (c.OwnerClassID == ObjectClass.Division 
                            && divisions.Select(v => v.ID).Contains(c.OwnerID)))
        .ToArrayAsync(cancellationToken);
    }


    private async Task<Subdivision[]> GetSubdivisionsByUserAsync(Guid userID, CancellationToken cancellationToken)
    {
        var division = await _users.AsNoTracking()
            .Include(c => c.Subdivision)
            .ThenInclude(c => c.ParentSubdivision)
            .Where(c => c.IMObjID == userID)
            .Select(c => c.Subdivision)
            .FirstOrDefaultAsync(cancellationToken);

        var divisions = new Queue<Subdivision>();
        while (division is not null)
        {
            divisions.Enqueue(division);
            division = division.ParentSubdivision;
        }

        return divisions.ToArray();
    }
}
