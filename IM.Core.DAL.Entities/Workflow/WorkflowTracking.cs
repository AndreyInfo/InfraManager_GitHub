using System;
using System.Collections.Generic;

namespace InfraManager.DAL.WF;

public class WorkflowTracking
{
    public Guid ID { get; init; }

    public Guid WorkflowSchemeID { get; set; }

    public string WorkflowSchemeIdentifier { get; set; }

    public string WorkflowSchemeVersion { get; set; }

    public int EntityClassID { get; set; }

    public Guid EntityID { get; set; }

    public DateTime UtcInitializedAt { get; set; }

    public DateTime? UtcTerminatedAt { get; set; }
    
    public virtual ICollection<WorkflowStateTracking> StateTrackings { get; init; }
}
