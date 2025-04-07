using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Software
{
    [ObjectClassMapping(ObjectClass.LicenceScheme)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.LicenceScheme_Create)]
    [OperationIdMapping(ObjectAction.Update, OperationID.LicenceScheme_Edit)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.LicenceScheme_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.LicenceScheme_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.LicenceScheme_Properties)]
    public partial class SoftwareLicenceScheme
    {
        public static Guid EmptyID = new("00000000-0000-0000-0000-000000000001");
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte SchemeType { get; set; }
        public byte LicensingObjectType { get; set; }
        public string LicenseCountPerObject { get; set; }
        public bool IsLinkLicenseToObject { get; set; }
        public bool IsLicenseAllHosts { get; set; }
        public bool IsLinkLicenseToUser { get; set; }
        public int? InstallationLimits { get; set; }
        public int? InstallationLimitPerVm { get; set; }
        public bool IsAllowInstallOnVm { get; set; }
        public bool IsLicenceByAccess { get; set; }
        public string AdditionalRights { get; set; }
        public int IncreaseForVm { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsCanHaveSubLicence { get; set; }
        public byte? CompatibilityTypeID { get; set; }

        public virtual ICollection<SoftwareLicenceSchemeProcessorCoeff> SoftwareLicenceSchemeCoefficients { get; set; }

    }
}
