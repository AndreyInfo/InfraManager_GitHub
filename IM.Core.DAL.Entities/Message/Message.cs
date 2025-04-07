using System;

namespace InfraManager.DAL.Message
{
    [ObjectClassMapping(ObjectClass.Message)]
    public class Message : IWorkflowEntity
    {
        public Guid IMObjID { get; set; }
        public DateTime UtcDateRegistered { get; set; }
        public string EntityStateID { get; set; }
        public string TargetEntityStateID { get; set; }
        public Guid? WorkflowSchemeID { get; set; }
        public string WorkflowSchemeVersion { get; set; }
        public string WorkflowSchemeIdentifier { get; set; }
        public string EntityStateName { get; set; }
        public DateTime? UtcDateClosed { get; set; }

        public byte Type { get; set; }
        public int Count { get; set; }
        public byte SeverityType { get; set; }
        public byte[] RowVersion { get; set; }

        public DateTime UtcDateModified 
        { 
            get => UtcDateRegistered; 
            set => UtcDateRegistered = value; 
        }
    }
}
