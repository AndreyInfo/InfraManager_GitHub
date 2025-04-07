using AutoMapper;
using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.UserImport.CommonSaveBLL.OrganizationStructure;
using InfraManager;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import;

internal class UserMapper : ImportMapper<IUserDetails,User>,ISelfRegisteredService<IImportMapper<IUserDetails, User>>
{
    public UserMapper(IDetailsLogic<IUserDetails> detailsLogic) : base(detailsLogic)
    {
    }
    protected override void AdditionalUpdateInit(ImportData<IUserDetails, User> importData, User entity)
    {
        if (importData.RestoreRemovedUsers)
            entity.Removed = false;
        
    }

    protected override void AdditionalInit(IMapper mapper, IUserDetails detail, User entity, Dictionary<IUserDetails, User> mappingDictionary)
    {
        base.AdditionalInit(mapper, detail, entity, mappingDictionary);
        if (detail is IUserHierarchyDetails hierarchyDetails)
        {
            var manager = hierarchyDetails.Manager;
            if (mappingDictionary.ContainsKey(manager))
                entity.Manager = mappingDictionary[manager];
            else
            {
                entity.Manager = mapper.Map<User>(manager);
                mappingDictionary[manager] = entity.Manager;
            }
        }
    }

    protected override Func<IUserDetails, IUserDetails?>? Recursion => x=>x?.Manager;

    protected override Action<User, User?>? SetRecursion => (x, y) => x.Manager = y;

    protected override void SetIgnoreFields(ObjectType flags, IMappingExpression<IUserDetails, User> map)
    {
        IgnoreFieldsIf(flags,ObjectType.UserFirstName,map,x=>x.Name);
        IgnoreFieldsIf(flags,ObjectType.UserLastName,map,x=>x.Surname);
        IgnoreFieldsIf(flags,ObjectType.UserPatronymic,map,x=>x.Patronymic);
        IgnoreFieldsIf(flags,ObjectType.UserEmail, map,x=>x.Email);
        IgnoreFieldsIf(flags,ObjectType.UserFax,map,x=>x.Fax);
        IgnoreFieldsIf(flags,ObjectType.UserLogin,map,x=>x.LoginName);
        IgnoreFieldsIf(flags,ObjectType.UserManager,map,x=>x.ManagerID);
        IgnoreFieldsIf(flags,ObjectType.UserNote,map,x=>x.Note);
        IgnoreFieldsIf(flags,ObjectType.UserNumber,map,x=>x.Number);
        IgnoreFieldsIf(flags,ObjectType.UserPager,map,x=>x.Pager);
        IgnoreFieldsIf(flags,ObjectType.UserPhone,map,x=>x.Phone);
        IgnoreFieldsIf(flags,ObjectType.UserPosition,map,x=>x.Position);
        IgnoreFieldsIf(flags,ObjectType.UserPhoneInternal,map,x=>x.Phone1,x=>x.Phone2,x=>x.Phone3,x=>x.Phone4);
        IgnoreFieldsIf(flags,ObjectType.UserExternalID,map,x=>x.ExternalID);
        IgnoreFieldsIf(flags,ObjectType.UserSID,map,x=>x.SID);
    }


    
}