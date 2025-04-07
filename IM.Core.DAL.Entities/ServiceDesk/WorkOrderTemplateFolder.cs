using Inframanager;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.ServiceDesk;

[ObjectClassMapping(ObjectClass.WorkOrderTemplateFolder)]
[OperationIdMapping(ObjectAction.Insert, OperationID.WorkOrderTemplateFolder_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.WorkOrderTemplateFolder_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.WorkOrderTemplateFolder_Properties)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.WorkOrderTemplateFolder_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.WorkOrderTemplateFolder_Properties)]
public class WorkOrderTemplateFolder : Catalog<Guid>
{
    public Guid? ParentID { get; set; }

    public byte[] RowVersion { get; set; }

    public virtual WorkOrderTemplateFolder Parent { get; }

    public virtual ICollection<WorkOrderTemplateFolder> SubFolder { get; }

    public virtual ICollection<WorkOrderTemplate> Templates { get; }
}
