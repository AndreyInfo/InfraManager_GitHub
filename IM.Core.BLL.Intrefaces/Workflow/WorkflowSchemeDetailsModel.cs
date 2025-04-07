using System;

namespace InfraManager.BLL.Workflow
{
    public class WorkflowSchemeDetailsModel
    {
        public Guid ID { get; init; }
        public  string Name { get; init; }
        public string Identifier { get; init; }
        public string Version { get; init; }
    }
}
