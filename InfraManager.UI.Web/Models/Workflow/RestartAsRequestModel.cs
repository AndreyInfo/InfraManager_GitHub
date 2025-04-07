using System;

namespace InfraManager.UI.Web.Models.Workflow;

public class RestartAsRequestModel
{
    public Guid EntityID { get; set; }
    public int EntityClassID { get; set; }
    public Guid WorkflowSchemeID { get; set; }
}