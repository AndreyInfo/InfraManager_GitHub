using System;
using Inframanager;
using InfraManager.DAL.Configuration;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.PortAdapter)]
[OperationIdMapping(ObjectAction.Insert, OperationID.PortAdapter_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.PortAdapter_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.PortTemplate_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.PortAdapter_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.PortAdapter_Properties)]
public class PortAdapter
{
    public Guid ID { get; init; }
    public Guid ObjectID { get; init; }
    public int PortNumber { get; init; }
    public int JackTypeID { get; init; }
    public int TechnologyID { get; init; }
    public string PortAddress { get; set; }
    public string Note { get; set; }

    public virtual ConnectorType JackType { get; init; }
    public virtual TechnologyType TechnologyType { get; init; }
}
