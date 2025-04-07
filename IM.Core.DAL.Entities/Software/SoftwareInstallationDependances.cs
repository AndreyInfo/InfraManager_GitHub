using System;

namespace InfraManager.DAL.Software
{
    public partial class SoftwareInstallationDependances
    {
        public Guid InstallationId { get; set; }
        public Guid DependantInstallationId { get; set; }

        public virtual SoftwareInstallation DependantInstallation { get; set; }
        public virtual SoftwareInstallation Installation { get; set; }
    }
}