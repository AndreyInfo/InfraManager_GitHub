using Inframanager.BLL;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    public class ChangeRequestReferenceListItemBase
    {
        public Guid IMObjID { get; init; }

        [ColumnSettings(0)]
        [Label(nameof(Resources.Number))]
        public int Number { get; init; }

        [ColumnSettings(1)]
        [Label(nameof(Resources.RFCType))]
        public string TypeName { get; init; }

        [ColumnSettings(2)]
        [Label(nameof(Resources.ShortDescription))]
        public string ShortDescription { get; init; }

        [ColumnSettings(3)]
        [Label(nameof(Resources.RFCState))]
        public string EntityStateName { get; init; }

        [ColumnSettings(4)]
        [Label(nameof(Resources.Owner))]
        public string Owner { get; init; }

        [ColumnSettings(5)]
        [Label(nameof(Resources.RFCPriority))]
        public string Priority { get; init; }

    }
}
