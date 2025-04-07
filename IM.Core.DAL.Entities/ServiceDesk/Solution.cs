using Inframanager;
using System;

namespace InfraManager.DAL.ServiceDesk;

[ObjectClassMapping(ObjectClass.Solution)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Solution_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Solution_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Solution_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.Solution_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Solution_Properties)]
public class Solution
{
    public Solution() 
    { 
        ID = Guid.NewGuid();
    }
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string HTMLDescription { get; init; }
    public byte[] RowVersion { get; init; }
}
