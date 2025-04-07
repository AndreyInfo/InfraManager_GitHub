using System.Linq;

namespace InfraManager.DAL.OrganizationStructure;

public interface IOwnerQuery
{
    IQueryable<OrganizationStructureItem> Query();
}