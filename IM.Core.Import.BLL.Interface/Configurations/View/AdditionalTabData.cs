namespace IM.Core.Import.BLL.Interface.Configurations.View
{
    public class AdditionalTabData
    {
        public bool UpdateSubdivision { get; set; }
        public int? SubdivisionDefaultOrganizationItemClassID { get; set; }
        public Guid? SubdivisionDefaultOrganizationItemID { get; set; }
        public Guid? UserDefaultOrganizationItemID { get; set; }
        public byte OrganizationComparison { get; set; }
        public byte SubdivisionComparison { get; set; }
        public byte UserComparison { get; set; }
        public bool RestoreRemovedUsers { get; set; }
        public bool UpdateLocation { get; set; }
        public byte LocationMode { get; set; }
        public Guid? LocationItemID { get; set; }
    }
}
