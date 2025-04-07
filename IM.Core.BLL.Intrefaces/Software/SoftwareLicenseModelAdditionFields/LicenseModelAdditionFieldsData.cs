using InfraManager.DAL.Software;
using System;

namespace InfraManager.BLL.Software.SoftwareLicenseModelAdditionFields
{
    public class LicenseModelAdditionFieldsData
    {
        public Guid SoftwareModelID { get; init; }
        public int? LicenseControlID { get; init; }
        public SoftwareModelLanguage? LanguageID { get; init; }
    }
}
