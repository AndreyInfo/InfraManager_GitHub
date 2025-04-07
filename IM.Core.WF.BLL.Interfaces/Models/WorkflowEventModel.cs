using System;

namespace IM.Core.WF.BLL.Interfaces.Models
{
    public class WorkflowEventModel
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
