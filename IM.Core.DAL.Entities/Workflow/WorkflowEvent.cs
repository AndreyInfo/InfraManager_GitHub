using System;

namespace InfraManager.DAL.WF
{
    public class WorkflowEvent
    {
        public long ID { get; set; }

        public Guid WorkflowID { get; set; }

        public DateTime UtcTimeStamp { get; set; }

        public byte Type { get; set; }

        public string Message { get; set; }

        public string StateID { get; set; }

        public string ActivityID { get; set; }
    }
}
