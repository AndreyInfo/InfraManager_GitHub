using System;

namespace InfraManager.DAL.Software
{
    public partial class SoftwareLicenceSchemeProcessorCoeff
    {
        public Guid LicenceSchemeId { get; set; }
        public Guid ProcessorTypeId { get; set; }
        public int Coefficient { get; set; }

        public virtual SoftwareLicenceScheme SoftwareLicenceScheme { get; set; }
    }
}