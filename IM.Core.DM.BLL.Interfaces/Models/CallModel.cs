using System;

namespace IM.Core.DM.BLL.Interfaces.Models
{
    public class CallModel
    {
        public Guid ID { get; set; }

        public Guid? WorkflowSchemeID { get; set; }

        public string EntityStateID { get; set; }

        public string EntityStateName { get; set; }

        public string WorkflowSchemeIdentifier { get; set; }

        public string WorkflowSchemeVersion { get; set; }

        public bool Removed { get; set; }

        public DateTime UtcDateModified { get; set; }
    }
}
