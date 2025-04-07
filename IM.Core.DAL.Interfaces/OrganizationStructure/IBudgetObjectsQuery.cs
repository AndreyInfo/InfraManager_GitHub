using System.Linq;

namespace InfraManager.DAL.OrganizationStructure
{
    public interface IBudgetObjectsQuery
    {
        IQueryable<BudgetObject> Query();
    }
}
