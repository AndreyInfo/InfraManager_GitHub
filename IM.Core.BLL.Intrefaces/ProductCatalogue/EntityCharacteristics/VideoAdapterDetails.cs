using System;

namespace InfraManager.BLL.ProductCatalogue.ModelCharacteristics;
public class VideoAdapterDetails : EntityCharacteristicsDetailsBase
{
    public string MemorySize { get; init; }
    public string VideoModeDescription { get; init; }
    public string ChipType { get; init; }
    public string DacType { get; init; }
}
