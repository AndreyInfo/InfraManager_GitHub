using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import;

internal class UserImportEntityData : IImportEntityData<IUserDetails,User,UserComparisonEnum>,ISelfRegisteredService<IImportEntityData<IUserDetails,User,UserComparisonEnum>>
{
    private readonly IUserImportRepository _userImportRepository;
    public UserImportEntityData(IUserImportRepository userImportRepository)
    {
        _userImportRepository = userImportRepository;
    }
    public Func<ICollection<IUserDetails>, IAdditionalParametersForSelect, CancellationToken, Task<IEnumerable<User>>> GetComparerFunction(UserComparisonEnum parameter, bool getRemoved)
    {
        
        //Фиксирование переменной
        return parameter switch
        {
            UserComparisonEnum.ByFullName => FixParameter<IUserDetails>(_userImportRepository.FromFullName, getRemoved),
            UserComparisonEnum.ByFirstNameLastName => FixParameter<IUserDetails>(_userImportRepository.FromFirstNameAndLastName, getRemoved),
            UserComparisonEnum.ByNumber => FixParameter<IUserDetails>(_userImportRepository.FromNumber, getRemoved),
            UserComparisonEnum.ByLogin => FixParameter<IUserDetails>((details, withRemoved) => _userImportRepository.FromLoginName(withRemoved, details.Select(x => x.LoginName)), getRemoved),
            UserComparisonEnum.BySID => FixParameter<IUserDetails>(_userImportRepository.FromSid, getRemoved),
            UserComparisonEnum.ByExternalID => FixParameter<IUserDetails>(_userImportRepository.FromExternalID, getRemoved),
            _=>throw new NotSupportedException()
        };
    }
    
    public Func<ICollection<IUserDetails>,IAdditionalParametersForSelect, CancellationToken, Task<IEnumerable<User>>> GetUserGetterForUniqueKeys(ObjectType type)
    {
        return type switch
        {
            ObjectType.UserLogin => FixParameter<IUserDetails>(
                (details, getRemoved) => _userImportRepository.FromLoginName(getRemoved, details.Select(x => x.LoginName)),
                false),
            ObjectType.UserEmail => FixParameter<IUserDetails>((detailsList,y)=> _userImportRepository.FromEmail(detailsList.Select(details => details.Email).ToList()),false),
            ObjectType.UserExternalID=>FixParameter<IUserDetails>((detailsList,y)=>_userImportRepository.FromExternalID(detailsList, y),false),
            _ => throw new NotImplementedException()
        };
    }
    
    private readonly Dictionary<ObjectType, ImportKeyData<IUserDetails,User>> _uniqueDetailsChecks = new()
    {
        {ObjectType.UserEmail, new ImportKeyData<IUserDetails,User>(new(){{x => new EmailKey(x.Email),x=>new EmailKey(x.Email)}}, nameof(User.Email))},
        {ObjectType.UserLogin, new ImportKeyData<IUserDetails,User>(new(){{x => new LoginKey(x.LoginName), x=>new LoginKey(x.LoginName)}}, nameof(User.LoginName))},
        {ObjectType.UserExternalID,new ImportKeyData<IUserDetails, User>(new(){{x=>new ExternalIDKey(x.ExternalID), x=>new ExternalIDKey(x.ExternalID)}},nameof(User.ExternalID))}
    };
    
    
    private static Func<ICollection<T>, IAdditionalParametersForSelect, CancellationToken, Task<IEnumerable<User>>> FixParameter<T>(
        Func<ICollection<T>, bool, IEnumerable<User>> data, bool getRemoved) =>
        (input,y,z) => Task.FromResult(data(input, getRemoved));

    
   
    
    public async Task<IEnumerable<IDuplicateKeyData<IUserDetails,User>>> GetUniqueKeys(ObjectType flags, bool getRemoved, CancellationToken token)
    {
        //Поиск открытых полей, которые должны быть уникальными
        return from uniqueCheck in _uniqueDetailsChecks 
            where flags.HasFlag(uniqueCheck.Key) 
            select new DuplicateKeyData<IUserDetails,User>(uniqueCheck.Value, GetUserGetterForUniqueKeys(uniqueCheck.Key));
    }
}