using System;

namespace InfraManager.BLL.ProductCatalogue.MaterialConsumptionRates;

public class MaterialConsumptionRateInputDetails
{

    public string DeviceModelID { get; init; }

    public int DeviceCategoryID { get; init; }

    public Guid MaterialModelID { get; init; }

    public decimal Amount { get; init; }
        
    public short? UseBWPrint { get; init; }

    public short? UseColorPrint { get; init; }

    public short? UsePhotoPrint { get; init; }
}