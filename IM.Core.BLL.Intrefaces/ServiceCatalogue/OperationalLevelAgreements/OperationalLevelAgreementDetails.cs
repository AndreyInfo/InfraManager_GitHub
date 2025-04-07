using System;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

public class OperationalLevelAgreementDetails : OperationalLevelAgreementData
{
    public int ID { get; init; }
    public Guid IMObjID { get; init; }
    public string CalendarWorkSchedule { get; init; }
    public string TimeZoneName { get; init; }
}