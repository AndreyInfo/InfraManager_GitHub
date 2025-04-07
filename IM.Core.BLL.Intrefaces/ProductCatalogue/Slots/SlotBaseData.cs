using System;

namespace InfraManager.BLL.ProductCatalogue.Slots;
public abstract class SlotBaseData
{
    public Guid ObjectID { get; init; }
    public int ObjectClassID { get; init; }
    public int SlotTypeID { get; init; }
    public int Number { get; set; }
}
