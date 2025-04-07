using System;
using System.Collections.Generic;

namespace InfraManager.DAL.ServiceDesk
{
    public class WorkFlowScheme
    {
        public Guid Id { get; set; }
        public Guid? WorkflowSchemeFolderID { get; set; }
        public int ObjectClassID { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public short MajorVersion { get; set; }
        public short MinorVersion { get; set; }
        public byte Status { get; set; }
        public DateTime UtcDateModified { get; set; }
        public Guid ModifierID { get; set; }
        public DateTime? UtcDatePublished { get; set; }
        public Guid? PublisherID { get; set; }
        public byte[] RowVersion { get; set; }
        public bool TraceIsEnabled { get; set; }
        public string LogicalScheme { get; set; }
        public string VisualScheme { get; set; }


        public virtual ICollection<ProblemType> ProblemTypes { get; set; }
        public virtual ICollection<WorkOrderType> WorkOrderTypes{ get; set; }
    }
}
