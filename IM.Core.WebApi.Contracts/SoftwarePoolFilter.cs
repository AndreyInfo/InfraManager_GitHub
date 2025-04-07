using System;

namespace InfraManager.BLL
{
    public class SoftwarePoolFilter
    {
        public byte SoftwareLicenceScheme { get; set; }
        public byte Type { get; set; }
        public Guid SoftwareDistributionCentreID { get; set; }
        public Guid SoftwareModelID { get; set; }
    }
}
