using System;

namespace InfraManager.BLL.Workflow
{
    public class WorkflowEntityData
    {
        public Guid Id { get; init; }
        public ObjectClass ClassId { get; init; }
        public string EntityState { get; init; }
    }
}
