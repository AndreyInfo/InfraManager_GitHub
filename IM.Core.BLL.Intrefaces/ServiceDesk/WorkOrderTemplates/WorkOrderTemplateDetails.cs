using InfraManager.DAL.ServiceDesk.WorkOrders.Templates;
using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

public class WorkOrderTemplateDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public Guid? InitiatorID { get; init; }
    public string InitiatorName { get; init; }
    public Guid WorkOrderPriorityID { get; init; }
    public Guid WorkOrderTypeID { get; init; }
    public Guid? ExecutorID { get; init; }
    public Guid? QueueID { get; init; }
    public Guid? WorkOrderTemplateFolderID { get; init; }
    public string FolderName { get; init; }
    public string UserField1 { get; init; }
    public string UserField2 { get; init; }
    public string UserField3 { get; init; }
    public string UserField4 { get; init; }
    public string UserField5 { get; init; }
    public Guid ParentID { get; init; }
    public string WorkOrderTypeName { get; init; }
    public string WorkOrderPriorityName { get; init; }
    
    public long DateStartedDelta { get; init; }
    public long DatePromisedDelta { get; init; }
    public byte[] RowVersion { get; init; }
    public int ManhoursNormInMinutes { get; init; }
    public Guid? FormID { get; init; }

    public ExecutorAssignmentType ExecutorAssignmentType { get; set; }
    public bool FlagTTZ { get; set; }
    public bool FlagTOZ { get; set; }
    public bool FlagServiceResponsible { get; set; }
    public bool FlagCalendarWorkSchedule { get; set; }

}

