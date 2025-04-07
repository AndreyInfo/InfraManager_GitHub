namespace InfraManager
{
    [Flags]
    public enum ObjectType : long
    {
        Nothing = 0,
        Organization = 1,
        OrganizationName = Organization << 1,
        OrganizationNote = Organization << 2,
        OrganizationExternalID = Organization << 3, //
        Subdivision = Organization << 12,
        SubdivisionName = Subdivision << 1,
        SubdivisionNote = Subdivision << 2,
        SubdivisionExternalID = Subdivision << 3, //
        SubdivisionOrganization = Subdivision << 4, //
        SubdivisionOrganizationExternalID = Subdivision << 5, //
        SubdivisionParent = Subdivision << 6, //
        SubdivisionParentExternalID = Subdivision << 7, //
        User = Subdivision << 12,
        UserLastName = User << 1,
        UserFirstName = User << 2,
        UserPatronymic = User << 3,
        UserLogin = User << 4,
        UserPosition = User << 5,
        UserPhone = User << 6,
        UserFax = User << 7,
        UserPager = User << 8,
        UserEmail = User << 9,
        UserNote = User << 10,
        UserSID = User << 11,
        UserOrganization = User << 12,
        UserSubdivision = User << 13,
        UserWorkplace = User << 14,
        UserNumber = User << 15,
        UserOrganizationExternalID = User << 16,
        UserSubdivisionExternalID = User << 17,
        UserExternalID = User << 18,
        UserPhoneInternal = User << 19,
        UserManager = User << 20,
    }
}