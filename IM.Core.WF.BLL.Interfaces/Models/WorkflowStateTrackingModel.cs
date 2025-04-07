using System;

namespace IM.Core.WF.BLL.Interfaces.Models
{
    public class WorkflowStateTrackingModel
    {
        public Guid? ExecutorID { get; set; }
        
        public DateTime? UtcLeavedAt { get; set; }

        public int? TimeSpanInWorkMinutes { get; set; }
        
        public string StateId { get; set; }
        
        public string StateName { get; set; }
        
        public DateTime UtcEnteredAt { get; set; }
        
        public string ExecutorName { get; set; }
    }
}
