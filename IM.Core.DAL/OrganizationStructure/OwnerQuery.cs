using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.OrganizationStructure;

internal class OwnerQuery : IOwnerQuery, ISelfRegisteredService<IOwnerQuery>
{
    private readonly DbSet<Organization> _organizations;
    private readonly DbSet<SideOrganization> _sideOrganizations;

    public OwnerQuery(
        DbSet<Organization> organizations,
        DbSet<SideOrganization> sideOrganizations)
    {
        _organizations = organizations;
        _sideOrganizations = sideOrganizations;
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

        return organizations.Concat(sideOrganizations);
    }
}