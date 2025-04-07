using System;

namespace InfraManager.DAL.Asset;
public abstract class SlotBase
{
    public Guid ObjectID { get; init; }
    public int ObjectClassID { get; init; }
    public int Number { get; init; }
    public int SlotTypeID { get; init; }

    public virtual SlotType SlotType { get; init; }
}
