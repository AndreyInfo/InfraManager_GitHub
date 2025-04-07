namespace InfraManager.BLL.Workflow
{
    public class WorkflowDetails
    {
        public WorkflowStateDetails[] States { get; init; }
        public bool ReadOnly { get; init; }
        public bool HasExternalEvents { get; init; }
        public string EntityStateName { get; init; }
    }
}
