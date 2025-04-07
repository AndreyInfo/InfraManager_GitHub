using System;

namespace InfraManager.BLL.Calls.DTO
{
    public class CallTypeDetails
    {
        public Guid ID { get; init; }

        public string Name { get; init; }

        public Guid? ParentCallTypeID { get; init; }

        public bool VisibleInWeb { get; init; }

        public Guid? EventHandlerCallTypeID { get; init; }

        public string EventHandlerName { get; init; }

        public byte[] Icon { get; init; }

        public string IconName { get; init; }

        public bool IsFixed { get; init; }
        
        public bool Removed { get; init; }

        public byte[] RowVersion { get; init; }

        public string WorkflowSchemeIdentifier { get; init; }

        public Guid? WorkflowSchemeId { get; init; }

        public bool UseWorkflowSchemeFromAttendance { get; init; }
        
        public string FullName { get; init; }
        
        public CallTypeDetails Parent { get; init; }
    }
}
