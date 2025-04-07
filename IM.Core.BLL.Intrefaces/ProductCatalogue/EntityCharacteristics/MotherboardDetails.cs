namespace InfraManager.BLL.ProductCatalogue.ModelCharacteristics;
public class MotherboardDetails : EntityCharacteristicsDetailsBase
{
    public string PrimaryBusType { get; init; }
    public string SecondaryBusType { get; init; }
    public string ExpansionSlots { get; init; }
    public string RamSlots { get; init; }
    public string MotherboardSize { get; init; }
    public string MotherboardChipset { get; init; }
    public string MaximumSpeed { get; init; }
}
