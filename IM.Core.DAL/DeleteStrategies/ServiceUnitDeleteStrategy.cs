using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.DeleteStrategies;

internal class ServiceUnitDeleteStrategy : 
    IDeleteStrategy<ServiceUnit>,
    ISelfRegisteredService<IDeleteStrategy<ServiceUnit>>
{
    private readonly IRepository<OrganizationItemGroup> _organizationItemGroups;
    private readonly DbSet<ServiceUnit> _serviceUnits;

    public ServiceUnitDeleteStrategy(IRepository<OrganizationItemGroup> organizationItemGroups,
        DbSet<ServiceUnit> serviceUnits)
    {
        _organizationItemGroups = organizationItemGroups;
        _serviceUnits = serviceUnits;
    }

    public void Delete(ServiceUnit entity)
    {
        entity.OrganizationItemGroups.ForEach(c => _organizationItemGroups.Delete(c));
        _serviceUnits.Remove(entity);
    }
}
