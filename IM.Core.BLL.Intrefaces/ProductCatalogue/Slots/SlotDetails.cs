using System;

namespace InfraManager.BLL.ProductCatalogue.Slots;
public class SlotDetails
{
    public Guid ObjectID { get; init; }
    public int ObjectClassID { get; init; }
    public int Number { get; init; }
    public string Note { get; init; }
    public int SlotTypeID { get; init; }
    public string SlotTypeName { get; init; }
    public Guid? AdapterID { get; init; }
}
