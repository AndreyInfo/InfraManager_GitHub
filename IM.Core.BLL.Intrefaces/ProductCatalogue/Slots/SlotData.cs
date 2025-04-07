using System;

namespace InfraManager.BLL.ProductCatalogue.Slots;

public class SlotData : SlotBaseData
{
    public string Note { get; init; }
    public Guid? AdapterID { get; init; }
}