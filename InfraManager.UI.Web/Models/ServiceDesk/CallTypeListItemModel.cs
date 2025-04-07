using System;

namespace InfraManager.UI.Web.Models.ServiceDesk
{
    public class CallTypeListItemModel
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string FullName { get; init; }
        public bool IsRFC { get; init; }
        public bool IsIncident { get; init; }
        public string ImageSource { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public Guid? ParentCallTypeID { get; init; }
    }
}
