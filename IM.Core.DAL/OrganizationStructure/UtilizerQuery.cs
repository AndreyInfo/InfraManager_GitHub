using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.OrganizationStructure;

internal class UtilizerQuery : IUtilizerQuery, ISelfRegisteredService<IUtilizerQuery>
{
    private readonly DbSet<Organization> _organizations;
    private readonly DbSet<SideOrganization> _sideOrganizations;
    private readonly DbSet<Subdivision> _subdivisions;
    private readonly DbSet<User> _users;
    private readonly DbSet<Group> _groups;

    public UtilizerQuery(
        DbSet<Organization> organizations,
        DbSet<SideOrganization> sideOrganizations,
        DbSet<Subdivision> subdivisions,
        DbSet<User> users,
        DbSet<Group> groups)
    {
        _organizations = organizations;
        _sideOrganizations = sideOrganizations;
        _subdivisions = subdivisions;
        _users = users;
        _groups = groups;
    }

    public IQueryable<OrganizationStructureItem> Query()
    {
        var organizations = _organizations.AsNoTracking()
            .Select(organization => new OrganizationStructureItem
            {
                ID = organization.ID,
                Name = DbFunctions.CastAsString(organization.Name),
                ClassID = ObjectClass.Organizaton,
            });
        
        var sideOrganizations = _sideOrganizations.AsNoTracking()
            .Select(sideOrganization => new OrganizationStructureItem
            {
                ID = sideOrganization.ID,
                Name = DbFunctions.CastAsString(sideOrganization.Name),
                ClassID = ObjectClass.SideOrganization,
            });

        var subdivisions = _subdivisions.AsNoTracking()
            .Select(subdivision => new OrganizationStructureItem
            {
                ID = subdivision.ID,
                Name = Subdivision.GetFullSubdivisionName(subdivision.ID),
                ClassID = ObjectClass.Substitution,
            });

        var users = _users.AsNoTracking()
            .Where(user => !user.Removed)
            .Where(User.ExceptSystemUsers)
            .Select(user => new OrganizationStructureItem
            {
                ID = user.IMObjID,
                Name = User.GetFullName(user.IMObjID),
                ClassID = ObjectClass.User,
            });

        var groups = _groups.AsNoTracking()
            .Where(Group.IsNotNullObject)
            .Select(group => new OrganizationStructureItem
            {
                ID = group.IMObjID,
                Name = DbFunctions.CastAsString(group.Name),
                ClassID = ObjectClass.Group,
            });

        return organizations
            .Concat(sideOrganizations)
            .Concat(subdivisions)
            .Concat(users)
            .Concat(groups);
    }
}