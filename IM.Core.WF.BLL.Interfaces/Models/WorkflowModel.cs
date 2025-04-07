using System;

namespace IM.Core.WF.BLL.Interfaces.Models
{
    public class WorkflowModel
    {
        public Guid ID { get; set; }

        public Guid WorkflowSchemeID { get; set; }

        public string WorkflowSchemeIdentifier { get; set; }

        public string WorkflowSchemeVersion { get; set; }

        public int EntityClassID { get; set; }

        public Guid EntityID { get; set; }

        public byte Status { get; set; }

        public DateTime UtcModifiedAt { get; set; }

        public string CurrentStateID { get; set; }

        public DateTime? UtcPlannedAt { get; set; }

        public byte[] Binaries { get; set; }
    }
}
