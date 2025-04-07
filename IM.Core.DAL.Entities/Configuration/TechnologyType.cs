using Inframanager;
using System.Collections.Generic;

namespace InfraManager.DAL.Configuration;

[ObjectClassMapping(ObjectClass.TechnologyType)]
[OperationIdMapping(ObjectAction.Insert, OperationID.TechnologyType_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.TechnologyType_Update)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.TechnologyType_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.TechnologyType_Properties)]
public class TechnologyType
{ 
    public int ID { get; init; }

    public string Name { get; set; }

    public int? ComplementaryID { get; set; }

    public virtual ICollection<TechnologyCompatibilityNode> TechnologyCompatibilityFrom { get; set; }

    public virtual ICollection<TechnologyCompatibilityNode> TechnologyCompatibilityTo { get; set; }
}
