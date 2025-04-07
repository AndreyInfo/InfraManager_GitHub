using System;

namespace InfraManager.BLL.ServiceCatalogue;

public class ServiceLevelAgreementData
{
    public Guid? FormID { get; init; }
    public string Name { get; init; }
    public string Note { get; init; }
    public string Number { get; init; }
    public DateTime? UtcStartDate { get; init; }
    public DateTime? UtcFinishDate { get; init; }
    public string TimeZoneID { get; init; }
    public Guid? CalendarWorkScheduleID { get; init; }
    public byte[] RowVersion { get; init; }

    public ConcludedDetails[] Concludeds { get; init; }
}