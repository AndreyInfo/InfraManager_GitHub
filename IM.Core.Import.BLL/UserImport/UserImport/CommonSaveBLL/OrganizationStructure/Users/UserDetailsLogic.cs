using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.UserImport.CommonSaveBLL.OrganizationStructure;
using InfraManager;

namespace IM.Core.Import.BLL.UserImport.UserImport.CommonSaveBLL.OrganizationStructure.Users;

public class UserDetailsLogic:IDetailsLogic<IUserDetails>, ISelfRegisteredService<IDetailsLogic<IUserDetails>>
{
    public ObjectType GetExcludedFields(IUserDetails details)
    {
        ObjectType flags = ObjectType.Nothing;
        if (!StringKey.IsSet(details.Name))
            flags |= ObjectType.UserFirstName;
        if (!StringKey.IsSet(details.Surname))
            flags |= ObjectType.UserLastName;
        if (!StringKey.IsSet(details.Patronymic))
            flags |= ObjectType.UserPatronymic;
        return flags;
    }
}