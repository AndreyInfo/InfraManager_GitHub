using System;

namespace InfraManager.BLL.ProductCatalogue.Slots;
public class SlotBaseKey
{
    public SlotBaseKey(Guid objectID, int number)
    {
        ObjectID = objectID;
        Number = number;
    }
    public Guid ObjectID { get; init; }
    public int Number { get; init; }
}
