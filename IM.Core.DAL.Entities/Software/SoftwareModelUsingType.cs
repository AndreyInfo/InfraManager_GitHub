using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Software;

[ObjectClassMapping(ObjectClass.SoftwareModelUsingType)]
[OperationIdMapping(ObjectAction.Insert, OperationID.SoftwareModelUsingType_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.SoftwareModelUsingType_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.SoftwareModelUsingType_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.SoftwareModelUsingType_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.SoftwareModelUsingType_Properties)]
public partial class SoftwareModelUsingType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Note { get; set; }
    public bool IsDefault { get; set; }
    public byte[] RowVersion { get; set; }

    public virtual ICollection<SoftwareModel> SoftwareModel { get; set; }
}