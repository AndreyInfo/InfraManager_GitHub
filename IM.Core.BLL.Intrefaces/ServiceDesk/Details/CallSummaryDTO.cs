using System;

namespace InfraManager.BLL.ServiceDesk.DTOs;

public class CallSummaryDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public Guid? ServiceItemID { get; init; }
    public Guid? ServiceAttendanceID { get; init; }
    public bool Visible { get; init; }
    public byte[] RowVersion { get; init; }
    public string ServiceName { get; init; }
    public string ServiceItemName { get; init; }
    public string ServiceAttendanceName { get; init; }
    public string ServiceCategoryName { get; init; }
    public string ItemOrAttendanceName
    {
        get 
        {
            // По клиентским сценариям не могут быть инициализированы сразу ServiceAttendanceID и ServiceItemID
            if (ServiceAttendanceID.HasValue)
                return ServiceAttendanceName;
            else if (ServiceItemID.HasValue)
                return ServiceItemName;

            return string.Empty;
        }
    }
}
