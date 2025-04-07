using System;

namespace InfraManager.BLL.ProductCatalogue.MaterialConsumptionRates;

public class MaterialConsumptionRateFilter
{
    public Guid ID { get; init; }
        
    public string DeviceModelID { get; init; }

    public int? DeviceCategoryID { get; init; }

    public Guid? MaterialModelID { get; init; }

    public string MaterialModelName { get; set; }
    
    public bool HasUseBWPrint { get; init; }

    public short? UseBWPrint { get; init; }

    public bool HasUseColorPrint { get; init; }
    public short? UseColorPrint { get; init; }
    
    public bool HasUsePhotoPrint { get; init; }

    public short? UsePhotoPrint { get; init; }
}