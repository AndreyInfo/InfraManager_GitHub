using Inframanager.BLL;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    public class CallReferenceListItemBase
    {
        public Guid IMObjID { get; init; }

        [ColumnSettings(0)]
        [Label(nameof(Resources.Number))]
        public int Number { get; init; }

        [ColumnSettings(1)]
        [Label(nameof(Resources.ShortDescription))]
        public string ShortDescription { get; init; }

        [ColumnSettings(2)]
        [Label(nameof(Resources.CallDatePromise))]
        public DateTime? UtcDatePromised { get; init; }

        [ColumnSettings(3)]
        [Label(nameof(Resources.CallState))]
        public string EntityStateName { get; init; }

        [ColumnSettings(4)]
        [Label(nameof(Resources.CallDateModified))]
        public DateTime UtcDateModified { get; init; }

        [ColumnSettings(5)]
        [Label(nameof(Resources.Client))]
        public string Client { get; init; }

        [ColumnSettings(6)]
        [Label(nameof(Resources.CallType))]
        public string TypeName { get; init; }
    }
}
