namespace InfraManager.UI.Web.Models.Asset
{
    public class TableAvailabilityModel
    {
        public readonly SoftwareDistributionAvailabilityModel SoftwareDistributionAvailabilityModel;
        public readonly SoftwareLicenceAvailabilityModel SoftwareLicenceAvailabilityModel;

        public TableAvailabilityModel(SoftwareDistributionAvailabilityModel softwareDistributionAvailabilityModel,
            SoftwareLicenceAvailabilityModel softwareLicenceAvailabilityModel)
        {
            SoftwareDistributionAvailabilityModel = softwareDistributionAvailabilityModel;
            SoftwareLicenceAvailabilityModel = softwareLicenceAvailabilityModel;
        }
    }
}
