using Inframanager;
using System.Collections.Generic;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.Medium)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Medium_Properties)]
public class Medium
{
    public int ID { get; init; }
    public string Name { get; init; }

    public virtual ICollection<ConnectorType> ConnectorTypes { get; }
}
