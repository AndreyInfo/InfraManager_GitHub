using System.Linq;

namespace InfraManager.DAL.OrganizationStructure;

public interface IUtilizerQuery
{
    IQueryable<OrganizationStructureItem> Query();
}