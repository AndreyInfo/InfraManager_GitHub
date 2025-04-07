using System;

namespace InfraManager.DAL.Software.Installation
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SoftwarePoolSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public byte SoftwareLicenceScheme { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid SoftwareDistributionCentreID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid SoftwareModelID { get; set; }
    }
}
