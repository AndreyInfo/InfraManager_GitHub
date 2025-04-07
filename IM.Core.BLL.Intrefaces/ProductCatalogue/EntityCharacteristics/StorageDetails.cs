using System;

namespace InfraManager.BLL.ProductCatalogue.ModelCharacteristics;
public class StorageDetails : EntityCharacteristicsDetailsBase
{
    public string FormattedCapacity { get; init; }
    public string RecordingSurfaces { get; init; }
    public string InterfaceType { get; init; }
}
