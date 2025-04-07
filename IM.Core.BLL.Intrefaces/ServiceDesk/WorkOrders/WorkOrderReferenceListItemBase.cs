using Inframanager.BLL;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public class WorkOrderReferenceListItemBase
    {
        public Guid IMObjID { get; init; }

        [ColumnSettings(0)]
        [Label(nameof(Resources.Number))]
        public int Number { get; init; }

        [ColumnSettings(1)]
        [Label(nameof(Resources.ShortDescription))]
        public string ShortDescription { get; init; }

        [ColumnSettings(2)]
        [Label(nameof(Resources.WorkOrderDatePromise))]
        public DateTime? UtcDatePromised { get; init; }

        [ColumnSettings(3)]
        [Label(nameof(Resources.WorkOrderState))]
        public string EntityStateName { get; init; }

        [ColumnSettings(4)]
        [Label(nameof(Resources.WorkOrderDateModified))]
        public DateTime UtcDateModified { get; init; }

        [ColumnSettings(5)]
        [Label(nameof(Resources.WorkOrderType))]
        public string TypeName { get; init; }
    }
}
