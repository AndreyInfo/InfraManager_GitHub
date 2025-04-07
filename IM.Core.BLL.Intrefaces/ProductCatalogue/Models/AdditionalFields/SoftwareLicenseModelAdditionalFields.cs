using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;
public class SoftwareLicenseModelAdditionalFields : IProductCatalogModelProperties
{
    public SoftwareLicenseModelAdditionalFields(SoftwareLicenseModel softwareLicenseModel)
    {
        SoftwareLicenseTypeID = softwareLicenseModel.SoftwareLicenceTypeID;
        SoftwareLicenseSchemeEnum = softwareLicenseModel.SoftwareLicenceSchemeEnum;
        SoftwareExecutionCount = softwareLicenseModel.SoftwareExecutionCount;
        LimitInHours = softwareLicenseModel.LimitInHours;
        DowngradeAvailable = softwareLicenseModel.DowngradeAvailable;
        SoftwareExecutionCountIsDefined = softwareLicenseModel.SoftwareExecutionCountIsDefined;
    }

    public short SoftwareLicenseTypeID { get; init; }
    public short SoftwareLicenseSchemeEnum { get; init; }
    public int SoftwareExecutionCount { get; init; }
    public int? LimitInHours { get; init; }
    public bool DowngradeAvailable { get; init; }
    public bool SoftwareExecutionCountIsDefined { get; init; }
}
