namespace InfraManager.UI.Web.Models.Asset
{
    public class SoftwareLicenceAvailabilityModel
    {
        public SoftwareLicenceAvailabilityModel(bool softwareLicence)
        {
            SoftwareLicence = softwareLicence;
        }

        public bool SoftwareLicence { get; }
    }
}
