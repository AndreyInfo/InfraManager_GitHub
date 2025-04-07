using System;

namespace InfraManager
{
    [Flags]
    public enum ConcordanceObjectType : long
    {
        Nothing = 0,
        Organization = 1,
        OrganizationName = Organization << 1 | Organization,
        OrganizationNote = Organization << 2 | Organization,
        OrganizationExternalID = Organization << 3 | Organization, //
        Subdivision = Organization << 12,
        SubdivisionName = Subdivision << 1 | Subdivision,
        SubdivisionNote = Subdivision << 2 | Subdivision,
        SubdivisionExternalID = Subdivision << 3 | Subdivision, //
        SubdivisionOrganization = Subdivision << 4 | Subdivision, //
        SubdivisionOrganizationExternalID = Subdivision << 5 | Subdivision, //
        SubdivisionParent = Subdivision << 6 | Subdivision, //
        SubdivisionParentExternalID = Subdivision << 7 | Subdivision, //
        User = Subdivision << 12,
        UserLastName = User << 1 | User,
        UserFirstName = User << 2 | User,
        UserPatronymic = User << 3 | User,
        UserLogin = User << 4 | User,
        UserPosition = User << 5 | User,
        UserPhone = User << 6 | User,
        UserFax = User << 7 | User,
        UserPager = User << 8 | User,
        UserEmail = User << 9 | User,
        UserNote = User << 10 | User,
        UserSID = User << 11 | User,
        UserOrganization = User << 12 | User,
        UserSubdivision = User << 13 | User,
        UserWorkplace = User << 14 | User,
        UserNumber = User << 15 | User,
        UserOrganizationExternalID = User << 16 | User,
        UserSubdivisionExternalID = User << 17 | User,
        UserExternalID = User << 18 | User,
        UserPhoneInternal = User << 19 | User,
        UserManager = User << 20 | User,
        All = long.MaxValue,
    }
}
