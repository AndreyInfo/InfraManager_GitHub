using InfraManager;
using InfraManager.DAL.Finance;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    internal class ProblemBudgetLookupQuery : ILookupQuery
    {
        private readonly DbSet<Problem> _problems;
        private readonly DbSet<BudgetUsage> _budgetUsages;
        private readonly DbSet<Budget> _budgets;
        private readonly IBudgetObjectsQuery _budgetObjects;

        public ProblemBudgetLookupQuery(
            DbSet<Problem> problems,
            DbSet<BudgetUsage> budgetUsages,
            DbSet<Budget> budgets,
            IBudgetObjectsQuery budgetObjects)
        {
            _problems = problems;
            _budgetUsages = budgetUsages;
            _budgets = budgets;
            _budgetObjects = budgetObjects;
        }

        public Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = from budgetUsage in _budgetUsages
                        join budgetObject in _budgetObjects.Query()
                            on budgetUsage.BudgetObjectId equals budgetObject.ID
                            into budgetObjectsAlias
                        from budgetObject in budgetObjectsAlias.DefaultIfEmpty()
                        join budget in _budgets
                            on budgetUsage.BudgetId equals budget.ID
                            into budgetAlias
                        from budget in budgetAlias.DefaultIfEmpty()
                        where budgetUsage.ObjectClass == ObjectClass.Problem
                            && _problems.Any(p => p.IMObjID == budgetUsage.ObjectId)
                        select new
                        {
                            ID = budget == null ? budgetObject.ID : budget.ID,
                            Name = budget == null ? budgetObject.Name : Budget.GetFullBudgetName(budget.ID)
                        };

            return query.Select(x => new ValueData { ID = x.ID.ToString(), Info = x.Name }).ToArrayAsync();
        }
    }
}
