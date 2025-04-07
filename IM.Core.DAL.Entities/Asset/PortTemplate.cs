using System;
using Inframanager;
using InfraManager.DAL.Configuration;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.ActivePort)]
[OperationIdMapping(ObjectAction.Insert, OperationID.None)]
[OperationIdMapping(ObjectAction.Update, OperationID.None)]
[OperationIdMapping(ObjectAction.Delete,OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails,OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class PortTemplate
{
    public Guid ObjectID { get; init; }
    public int ClassID { get; init; }
    public int PortNumber { get; init; }
    public int JackTypeID { get; init; }
    public int TechnologyID { get; init; }

    public virtual ConnectorType JackType { get; init; }
    public virtual TechnologyType TechnologyType { get; init; }
}