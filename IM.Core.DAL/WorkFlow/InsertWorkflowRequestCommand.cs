using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace InfraManager.DAL.WorkFlow
{
    internal class InsertWorkflowRequestCommand : WorkflowRequestCommand,
        IInsertWorkflowRequestCommand, 
        ISelfRegisteredService<IInsertWorkflowRequestCommand>
    {
        public InsertWorkflowRequestCommand(CrossPlatformDbContext db) : base(db)
        {
        }

        protected override string CreateSqlCommand(StoreObjectIdentifier table, string columnName, Guid workflowID)
        {
            return $"insert into {table.Schema}.{table.Name} ({columnName}) values ('{workflowID}')";
        }
    }
}
