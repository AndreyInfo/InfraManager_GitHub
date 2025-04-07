using System;

namespace InfraManager.BLL.ServiceCatalogue
{
    public class ServiceLevelAgreementDetails : ServiceLevelAgreementData
    {
        public Guid ID { get; init; }
        public string CalendarWorkSchedule { get; init; }
        public string TimeZoneName { get; init; }
    }
}
