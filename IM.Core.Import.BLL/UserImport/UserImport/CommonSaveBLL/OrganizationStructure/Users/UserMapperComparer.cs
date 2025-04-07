using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import;

internal class UserMapperComparer : ImportMapperComparer<IUserDetails,User>, ISelfRegisteredService<IImportMapperComparer<IUserDetails,User>>
{
    protected override List<KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>> Comparers { get; } = new()
    {
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserFirstName, (x, y) =>!StringKey.IsSet(x.Name) || x.Name == y.Name),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserLastName, (x, y) => !StringKey.IsSet(x.Surname) || x.Surname == y.Surname),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserPatronymic, (x, y) =>!StringKey.IsSet(x.Patronymic) || x.Patronymic == y.Patronymic),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserEmail, (x, y) => x.Email == y.Email),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserFax, (x, y) => x.Fax == y.Fax),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserLogin, (x, y) => x.LoginName == y.LoginName),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserNote, (x, y) => x.Note == y.Note),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserNumber, (x, y) => x.Number == y.Number),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserPager, (x, y) => x.Pager == y.Pager),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserPhone, (x, y) => x.Phone == y.Phone),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserPosition, (x, y) => x.PositionID == y.PositionID),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.User, (x, y) => x.WorkPlaceID == y.WorkplaceID),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserPhoneInternal, (x, y) => x.Phone1 == y.Phone1),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserExternalID, (x, y) => x.ExternalID == y.ExternalID),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserSID, (x, y) => x.SID == y.SID),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.User, (x, y) => x.SubdivisionID == y.SubdivisionID),
        new KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>(ObjectType.UserManager,(x,y)=>x.ManagerID == y.ManagerID)
    };


    protected override void SetIgnoreCompareFields(ObjectType flags, List<KeyValuePair<ObjectType, Func<IUserDetails, User, bool>>> fieldComparers)
    {
        IgnoreComparerIf(flags,ObjectType.UserEmail, fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserFax,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserLogin,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserManager,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserNote,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserNumber,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserPager,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserPatronymic,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserLastName,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserPhone,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserPosition,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.User,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserFirstName,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserPhoneInternal,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserExternalID,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.UserSID,fieldComparers);
        IgnoreComparerIf(flags,ObjectType.User,fieldComparers);
    }
}