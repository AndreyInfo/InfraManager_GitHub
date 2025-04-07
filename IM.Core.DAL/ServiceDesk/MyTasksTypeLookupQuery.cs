using InfraManager;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    internal class MyTasksTypeLookupQuery : ILookupQuery
    {
        private readonly IEnumerable<IQueryIssueTypes> _issueTypeQueries;

        public MyTasksTypeLookupQuery(IEnumerable<IQueryIssueTypes> issueTypeQueries)
        {
            _issueTypeQueries = issueTypeQueries;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            IQueryable<IssueType> query = null;
            foreach(var issueTypeQuery in _issueTypeQueries)
            {
                query = query == null ? issueTypeQuery.Query() : query.Concat(issueTypeQuery.Query());
            }

            return Array.ConvertAll(
                await query.ToArrayAsync(cancellationToken),
                x => new ValueData
                {
                    ID = x.ID.ToString(),
                    Info = x.Name
                });
        }
    }
}

