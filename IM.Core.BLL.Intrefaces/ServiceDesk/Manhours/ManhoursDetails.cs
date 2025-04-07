using System;

namespace InfraManager.BLL.ServiceDesk.Manhours
{
    public class ManhoursDetails
    {
        public Guid ID { get; init; }
        public int Value { get; init; }
        public Guid WorkID { get; init; }
        public DateTime UtcDate { get; init; }
    }
}
