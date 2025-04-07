using InfraManager;
using InfraManager.DAL.Finance;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal class CallBudgetLookupQuery : ILookupQuery
    {
        private readonly DbSet<Call> _calls;
        private readonly DbSet<BudgetUsage> _budgetUsages;
        private readonly DbSet<Budget> _budgets;
        private readonly IBudgetObjectsQuery _budgetObjects;

        public CallBudgetLookupQuery(
            DbSet<Call> calls,
            DbSet<BudgetUsage> budgetUsages,
            DbSet<Budget> budgets,
            IBudgetObjectsQuery budgetObjects)
        {
            _calls = calls;
            _budgetUsages = budgetUsages;
            _budgets = budgets;
            _budgetObjects = budgetObjects;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
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
                        where budgetUsage.ObjectClass == ObjectClass.Call
                            && _calls.Any(p => p.IMObjID == budgetUsage.ObjectId)
                        select new
                        {
                            ID = budget == null ? budgetObject.ID : budget.ID,
                            Name = budget == null ? budgetObject.Name : Budget.GetFullBudgetName(budget.ID)
                        };

            return Array.ConvertAll(await query.Distinct().ToArrayAsync(cancellationToken), x => new ValueData { ID = x.ID.ToString(), Info = x.Name });
        }
    }
}

