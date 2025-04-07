using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace InfraManager.DAL.WorkFlow
{
    internal class DeleteWorkflowRequestCommand : WorkflowRequestCommand,
        IDeleteWorkflowRequestCommand, 
        ISelfRegisteredService<IDeleteWorkflowRequestCommand>
    {
        public DeleteWorkflowRequestCommand(CrossPlatformDbContext db) : base(db)
        {
        }

        protected override string CreateSqlCommand(StoreObjectIdentifier table, string columnName, Guid workflowID)
        {
            return $"delete from {table.Schema}.{table.Name} where {columnName} = '{workflowID}'";
        }
    }
}
