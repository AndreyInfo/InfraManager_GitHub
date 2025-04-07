using Inframanager;
using InfraManager.DAL.ServiceCatalogue;
using System;

namespace InfraManager.DAL.ServiceDesk;

/// <summary>
/// Краткое описание заявки
/// </summary>
[OperationIdMapping(ObjectAction.Insert, OperationID.CallSummary_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.CallSummary_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.CallSummary_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.CallSummary_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.CallSummary_Properties)]
public class CallSummary
{
    public Guid ID { get; set; }

    public string Name { get; set; }

    public Guid? ServiceItemID { get; set; }

    public Guid? ServiceAttendanceID { get; set; }

    public byte[] RowVersion { get; set; }

    public bool Visible { get; set; }

    public virtual ServiceItem ServiceItem { get; set; }

    public virtual ServiceAttendance ServiceAttendance { get; set; }
}
