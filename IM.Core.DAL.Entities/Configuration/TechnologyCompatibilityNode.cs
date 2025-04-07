using Inframanager;

namespace InfraManager.DAL.Configuration;

[OperationIdMapping(ObjectAction.Insert, OperationID.TechnologyType_Update)]
[OperationIdMapping(ObjectAction.Update, OperationID.TechnologyType_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.TechnologyType_Update)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.TechnologyType_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.TechnologyType_Properties)]
public class TechnologyCompatibilityNode
{
    protected TechnologyCompatibilityNode()
    {
    }

    public TechnologyCompatibilityNode(int techIDFrom, int techIDTo)
    {
        TechIDFrom = techIDFrom;
        TechIDTo = techIDTo;
    }

    public int TechIDFrom { get; init; }
    public virtual TechnologyType TechnologyTypeFrom { get; init; }

    public int TechIDTo { get; init; }
    public virtual TechnologyType TechnologyTypeTo { get; init; }
}
