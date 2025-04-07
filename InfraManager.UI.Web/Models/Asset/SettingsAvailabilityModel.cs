namespace InfraManager.UI.Web.Models.Asset
{
    public sealed class SettingsAvailabilityModel : SoftwareDistributionAvailabilityModel
    {
        public SettingsAvailabilityModel(bool orgStructure, bool softwareDistributionCentres)
            : base(softwareDistributionCentres)
        {
            OrgStructure = orgStructure;
        }

        public bool OrgStructure { get; }
        public bool NoneAvailable => !OrgStructure && !SoftwareDistributionCentres;
    }
}
