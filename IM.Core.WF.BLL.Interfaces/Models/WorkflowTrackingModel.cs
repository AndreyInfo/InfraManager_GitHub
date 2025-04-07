using System;

namespace IM.Core.WF.BLL.Interfaces.Models
{
    public class WorkflowTrackingModel
    {
        public Guid ID { get; init; }

        public Guid WorkflowSchemeID { get; init; }

        public string WorkflowSchemeIdentifier { get; init; }

        public string WorkflowSchemeVersion { get; init; }

        public int EntityClassID { get; init; }

        public Guid EntityID { get; init; }

        public DateTime UtcInitializedAt { get; init; }

        public DateTime? UtcTerminatedAt { get; init; }

        public string EntityCategoryName { get; init; }

        public string EntityNamee()
        {
            var stringResult = EntityCategoryName;

            if (Number != 0)
            {
                return stringResult += $" № {Number} {SummaryName}";
            }

            return stringResult + $" {SummaryName}";
        }

        public string EntityName => EntityNamee();

        public string EntityTypeName { get; init; }
        
        public string WorkflowSchemeName { get; init; }
        
        public int Number { get; init; }
        
        public string SummaryName { get; init; }
        public string State { get; init; }
    }
}
