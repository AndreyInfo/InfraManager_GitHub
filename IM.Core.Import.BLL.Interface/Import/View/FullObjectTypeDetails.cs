namespace IM.Core.Import.BLL.Interface.Import.View
{
    public class FullObjectTypeDetails
    {
        public bool Organization { get; init; }
        public bool OrganizationName { get; init; }
        public bool OrganizationNote { get; init; }
        public bool OrganizationExternalID { get; init; }
        public bool Subdivision { get; init; }
        public bool SubdivisionName { get; init; }
        public bool SubdivisionNote { get; init; }
        public bool SubdivisionExternalID { get; init; }
        public bool SubdivisionOrganization { get; init; }
        public bool SubdivisionOrganizationExternalID { get; init; }
        public bool SubdivisionParent { get; init; }
        public bool SubdivisionParentExternalID { get; init; }
        public bool User { get; init; }
        public bool UserLastName { get; init; }
        public bool UserFirstName { get; init; }
        public bool UserPatronymic { get; init; }
        public bool UserLogin { get; init; }
        public bool UserPosition { get; init; }
        public bool UserPhone { get; init; }
        public bool UserFax { get; init; }
        public bool UserPager { get; init; }
        public bool UserEmail { get; init; }
        public bool UserNote { get; init; }
        public bool UserSID { get; init; }
        public bool UserOrganization { get; init; }
        public bool UserSubdivision { get; init; }
        public bool UserNumber { get; init; }
        public bool UserOrganizationExternalID { get; init; }
        public bool UserSubdivisionExternalID { get; init; }
        public bool UserExternalID { get; init; }
        public bool UserWorkplace { get; init; } = true;
        public bool UserPhoneInternal { get; init; }
        public bool UserManager { get; init; }
    }
}
