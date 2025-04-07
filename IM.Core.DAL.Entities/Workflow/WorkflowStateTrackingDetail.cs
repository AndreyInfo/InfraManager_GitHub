using System;

namespace InfraManager.DAL.WF
{
    public class WorkflowStateTrackingDetail
    {
        public Guid Id { get; init; }

        public DateTime? NextUtcDate { get; set; }

        public int? TimeSpanInWorkMinutes { get; set; }

        public int? StageTimeSpanInMinutes { get; set; }

        public int? StageTimeSpanInWorkMinutes { get; set; }
    }
}
