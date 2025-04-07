using Inframanager;
using System;

namespace InfraManager.DAL.Software;

[ObjectClassMapping(ObjectClass.SoftwareModel)]
[OperationIdMapping(ObjectAction.Insert, OperationID.SoftwareModel_Update)]
[OperationIdMapping(ObjectAction.Update, OperationID.SoftwareModel_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.SoftwareModel_Update)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.SoftwareModel_Properties)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.SoftwareModel_Properties)]
public class SoftwareModelDependency
{
    public Guid ParentSoftwareModelID { get; set; }
    public Guid ChildSoftwareModelID { get; set; }
    public SoftwareModelDependencyType Type { get; set; }

    public virtual SoftwareModel SoftwareModelParent { get; }
    public virtual SoftwareModel SoftwareModelChild { get; }
}
