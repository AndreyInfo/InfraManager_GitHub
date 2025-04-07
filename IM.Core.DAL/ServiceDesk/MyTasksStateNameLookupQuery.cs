using InfraManager;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    internal class MyTasksStateNameLookupQuery : ILookupQuery
    {
        private readonly IEnumerable<ITaskStateNameQuery> _queries;

        public MyTasksStateNameLookupQuery(IEnumerable<ITaskStateNameQuery> queries)
        {
            _queries = queries;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var result = new List<ValueData>();

            foreach(var query in _queries)
            {
                var data = await query.ExecuteAsync(cancellationToken);
                result.AddRange(data.Where(x => !result.Any(r => r.ID == x.ID)));
            }

            return result.ToArray();
        }
    }
}

