using System;

namespace InfraManager.BLL.ProductCatalogue.SlotTemplates;
public class SlotTemplateDetails
{
    public Guid ObjectID { get; init; }
    public int ObjectClassID { get; init; }
    public int Number { get; init; }
    public int SlotTypeID { get; init; }
    public string SlotTypeName { get; init; }
}
