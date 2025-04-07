using Inframanager;
using System;

namespace InfraManager.DAL;

[ObjectClassMapping(ObjectClass.CreepingLine)]
[OperationIdMapping(ObjectAction.Insert, OperationID.CreepingLine_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.CreepingLine_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.CreepingLine_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.CreepingLine_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.CreepingLine_Properties)]
public class CreepingLine
{
    public static int MaxNameLength => 550;
    
    public CreepingLine()
    {
        ID = Guid.NewGuid();
    }
    
    public Guid ID { get; init; }
    public string Name { get; set; }
    public bool Visible { get; set; }
    public byte[] RowVersion { get; set; }
}
