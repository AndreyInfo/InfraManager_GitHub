using Inframanager;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk.WorkOrders.Templates;
using System;

namespace InfraManager.DAL.ServiceDesk.WorkOrders;

[ObjectClassMapping(ObjectClass.WorkOrderTemplate)]
[OperationIdMapping(ObjectAction.Insert, OperationID.WorkOrderTemplate_Add)]
[OperationIdMapping(ObjectAction.Delete, OperationID.WorkOrderTemplate_Delete)]
[OperationIdMapping(ObjectAction.Update, OperationID.WorkOrderTemplate_Update)]
[OperationIdMapping(ObjectAction.InsertAs, OperationID.WorkOrderTemplate_AddAs)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.WorkOrderTemplate_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.WorkOrderTemplate_Properties)]
public class WorkOrderTemplate : Catalog<Guid>, IFormBuilder
{
    public Guid? FolderID { get; init; }
    public Guid? InitiatorID { get; init; }
    public Guid WorkOrderTypeID { get; init; }
    public virtual WorkOrderType Type { get; init; }
    public Guid WorkOrderPriorityID { get; init; }
    public virtual WorkOrderPriority Priority { get; init; }
    public Guid? ExecutorID { get; init; }
    public Guid? QueueID { get; init; }
    public ExecutorAssignmentType ExecutorAssignmentType { get; init; }
    public long DateStartedDelta { get; init; }
    public long DatePromisedDelta { get; init; }
    public string UserField1 { get; init; }
    public string UserField2 { get; init; }
    public string UserField3 { get; init; }
    public string UserField4 { get; init; }
    public string UserField5 { get; init; }
    public string Description { get; init; }
    public int ManhoursNormInMinutes { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid? FormID { get; init; }
    public virtual Form Form { get; }
    public virtual User Initiator { get; init; }
    public virtual WorkOrderTemplateFolder Folder { get; init; }
}
