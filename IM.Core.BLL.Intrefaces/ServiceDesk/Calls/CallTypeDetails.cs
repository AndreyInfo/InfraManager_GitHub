using System;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public class CallTypeDetails
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string FullName { get; init; }
        public bool IsRFC { get; init; }
        public bool IsIncident { get; init; }
        public bool HasNoImage { get; init; }
        public Guid? ParentCallTypeID { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
    }
}
