using System;

namespace InfraManager.DAL.ServiceDesk.Manhours
{
    public class ManhoursEntry
    {
        public Guid ID { get; }
        public Guid WorkID { get; }
        public int Value { get; set; }
        public DateTime UtcDate { get; set; }

        public ManhoursEntry()
        {
        }

        internal ManhoursEntry(Guid workID, int value, DateTime utcDate)
        {
            ID = Guid.NewGuid();
            WorkID = workID;
            Value = value;
            UtcDate = utcDate;
        }
    }
}