using System;

namespace InfraManager.BLL.ServiceDesk.Manhours
{
    public class ManhourData
    {
        public Guid? ID { get; init; }
        public DateTime UtcDate { get; init; }
        public int Value { get; init; }
    }
}
