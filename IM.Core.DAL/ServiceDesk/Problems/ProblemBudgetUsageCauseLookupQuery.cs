using InfraManager;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Finance;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    internal class ProblemBudgetUsageCauseLookupQuery : ILookupQuery
    {
        private readonly DbSet<Problem> _problems;
        private readonly DbSet<BudgetUsageCause> _budgetUsageCauses;
        private readonly DbSet<ServiceLevelAgreement> _serviceLevelAgreements;

        public ProblemBudgetUsageCauseLookupQuery(
            DbSet<Problem> problems,
            DbSet<BudgetUsageCause> budgetUsageCauses,
            DbSet<ServiceLevelAgreement> serviceLevelAgreements)
        {
            _problems = problems;
            _budgetUsageCauses = budgetUsageCauses;
            _serviceLevelAgreements = serviceLevelAgreements;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var budgetUsageCausesQuery = _budgetUsageCauses
                .Where(cause => cause.ObjectClass == ObjectClass.Problem
                    && _problems.Any(p => p.IMObjID == cause.ObjectId));
            var budgetUsageCauses = await budgetUsageCausesQuery
                .Select(cause => new ValueData { ID = cause.Text, Info = cause.Text })
                .ToArrayAsync();

            var slaList = await
                (from sla in _serviceLevelAgreements
                 join cause in budgetUsageCausesQuery on sla.ID equals cause.Sla.ID
                 select new { sla.ID, sla.Name }).ToArrayAsync();

            return budgetUsageCauses
                .Union(slaList.Select(s => new ValueData { ID = s.ID.ToString(), Info = s.Name }))
                .ToArray();
        }
    }
}
