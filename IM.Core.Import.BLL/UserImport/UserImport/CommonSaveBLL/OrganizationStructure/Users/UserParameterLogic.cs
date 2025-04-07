using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.View;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Users;

namespace IM.Core.Import.BLL.Import;

internal class UserParameterLogic : IImportParameterLogic<IUserDetails,User, UserComparisonEnum>, ISelfRegisteredService<IImportParameterLogic<IUserDetails,User, UserComparisonEnum>>
{
    public Func<IUserDetails, IIsSet> GetDetailsKey(UserComparisonEnum parameter)
    {
        return parameter switch
        {
            UserComparisonEnum.ByFullName => x => new FIOKey(x.FullName),
            UserComparisonEnum.ByFirstNameLastName => x => new SurnameNameKey(x.Surname,x.Name),
            UserComparisonEnum.ByNumber => x => new TabNumKey(x.Number),
            UserComparisonEnum.ByLogin => x =>new LoginKey(x.LoginName),
            UserComparisonEnum.BySID => x => new SIDKey(x.SID),
            UserComparisonEnum.ByExternalID => x => new ExternalIDKey(x.ExternalID),
            _ => throw new NotSupportedException()
        };
    }

    public Func<IUserDetails, bool> ValidateBeforeInitFunc(AdditionalTabDetails parameter)
    {
        
        return (UserComparisonEnum)parameter.UserComparison switch
        {
            UserComparisonEnum.ByFullName => x => !(FIOKey.IsSet(x.FullName) && SurnameNameKey.IsSet(x.Surname,x.Name)),
            UserComparisonEnum.ByFirstNameLastName => x => !SurnameNameKey.IsSet(x.Surname,x.Name),
            UserComparisonEnum.ByNumber => x => !TabNumKey.IsSet(x.Number),
            UserComparisonEnum.ByLogin => x =>  !LoginKey.IsSet(x.LoginName),
            UserComparisonEnum.BySID => x =>  !SIDKey.IsSet(x.SID),
            UserComparisonEnum.ByExternalID => x =>  !ExternalIDKey.IsSet(x.ExternalID),
            _=>throw new NotSupportedException()
        };
    }

    public ImportKeyData<IUserDetails,User> GetModelKey(UserComparisonEnum parameter)
    {
        return parameter switch
        {
            UserComparisonEnum.ByFullName => new ImportKeyData<IUserDetails,User>(new(){{x => new FIOKey(x.FullName),x=>new FIOKey(x.FullName)}}, nameof(User.FullName)),
            UserComparisonEnum.ByFirstNameLastName => new ImportKeyData<IUserDetails,User>(new(){{x => new SurnameNameKey(x.Surname,x.Name), x=>new SurnameNameKey(x.Surname,x.Name)}}, "Имя и фамилия"),
            UserComparisonEnum.ByNumber => new ImportKeyData<IUserDetails,User>(new(){{x => new TabNumKey(x.Number),x=>new TabNumKey(x.Number)}}, nameof(User.Number)),
            UserComparisonEnum.ByLogin => new ImportKeyData<IUserDetails,User>(new(){{x => new LoginKey(x.LoginName),x=>new LoginKey(x.LoginName)}}, nameof(User.LoginName)),
            UserComparisonEnum.BySID => new ImportKeyData<IUserDetails,User>(new(){{x => new SIDKey(x.SID),x=>new SIDKey(x.SID)}}, nameof(User.SID)),
            UserComparisonEnum.ByExternalID => new ImportKeyData<IUserDetails,User>(new(){{x => new ExternalIDKey(x.ExternalID),x=>new ExternalIDKey(x.ExternalID)}}, nameof(User.ExternalID)),
            _ => throw new NotSupportedException()
        };
    }

    public Func<IUserDetails, bool> ValidateAfterInitFunc()
    {
        return x => !x.SubdivisionID.HasValue;
    }

    public Func<User, bool> ValidateBeforeCreate()
    {
        return x => !(SurnameNameKey.IsSet(x.Surname, x.Name) && x.SubdivisionID.HasValue);
    }
}