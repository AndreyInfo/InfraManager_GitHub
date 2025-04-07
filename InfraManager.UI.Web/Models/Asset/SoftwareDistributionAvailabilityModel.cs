namespace InfraManager.UI.Web.Models.Asset
{
    public class SoftwareDistributionAvailabilityModel
    {
        public SoftwareDistributionAvailabilityModel(bool softwareDistributionCentres)
        {
            SoftwareDistributionCentres = softwareDistributionCentres;
        }

        public bool SoftwareDistributionCentres { get; }
    }
}