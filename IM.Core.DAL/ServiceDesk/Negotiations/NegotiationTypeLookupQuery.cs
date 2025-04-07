using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    internal class NegotiationTypeLookupQuery : ILookupQuery
    {
        private readonly DbSet<CallType> _callTypes;
        private readonly DbSet<WorkOrderType> _workOrderTypes;
        private readonly DbSet<ProblemType> _problemTypes;

        public NegotiationTypeLookupQuery(
            DbSet<CallType> callTypes,
            DbSet<WorkOrderType> workOrderTypes,
            DbSet<ProblemType> problemTypes
            )
        {
            _callTypes = callTypes;
            _workOrderTypes = workOrderTypes;
            _problemTypes = problemTypes;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = _callTypes
                .Where(ct => ct.ID != Guid.Empty)
                .Select(ct => new { ct.ID, Info = DbFunctions.CastAsString(CallType.GetFullCallTypeName(ct.ID)) })
                .Union(_workOrderTypes
                .Where(wt => wt.ID != Guid.Empty)
                .Select(wt => new { wt.ID, Info = DbFunctions.CastAsString(wt.Name) }))
                .Union(_problemTypes
                .Where(pt => pt.ID != Guid.Empty)
                .Select(pt => new { pt.ID, Info = DbFunctions.CastAsString(ProblemType.GetFullProblemTypeName(pt.ID)) }))
                .ToArrayAsync(cancellationToken);

            return Array.ConvertAll(await query, x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}

