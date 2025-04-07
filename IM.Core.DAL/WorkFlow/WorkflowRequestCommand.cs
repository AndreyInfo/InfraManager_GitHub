using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.WorkFlow
{
    internal abstract class WorkflowRequestCommand
    {
        private readonly DbContext _dbContext;

        protected WorkflowRequestCommand(CrossPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> ExecuteAsync(Guid workflowID, CancellationToken cancellationToken = default)
        {
            var sql = CreateSqlCommand(
                _dbContext.Model<WorkflowRequest>().GetTableIdentifier(),
                _dbContext.ColumnName<WorkflowRequest, Guid>(x => x.Id),
                workflowID);
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }

        protected abstract string CreateSqlCommand(StoreObjectIdentifier table, string columnName,Guid workflowID);
    }
}
