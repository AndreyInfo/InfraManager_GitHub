using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.Problems
{
    public class ProblemTypeDetailsModel
    {
        public Guid ID { get; set; }
        
        public string Name { get; set; }
        
        public string FullName { get; set; }
        
        public Guid? ParentID { get; set; }
        
        public string WorkflowSchemeIdentifier { get; set; }
        
        public string RowVersion { get; set; }
        public Guid? FormID { get; init; }
    }
}
