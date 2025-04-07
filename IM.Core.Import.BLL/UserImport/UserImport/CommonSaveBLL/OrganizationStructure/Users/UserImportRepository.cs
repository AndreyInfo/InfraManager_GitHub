using AutoMapper;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Users;

namespace IM.Core.Import.BLL.Import;

internal class UserImportRepository : IUserImportRepository, ISelfRegisteredService<IUserImportRepository>
{
    private readonly IAllUserQuery _userRepository;
    private readonly IGetUserByCriteriaQuery _userByCriteria;
    private readonly IMapper _mapper;
    

    public UserImportRepository(IAllUserQuery userRepository, 
        IMapper mapper, IGetUserByCriteriaQuery userByCriteria)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _userByCriteria = userByCriteria;
    }

    public IEnumerable<User> FromFullName(ICollection<IUserDetails> details, bool getRemoved)
    {
        //todo:сделать async
        var keys = _mapper.Map<FIO[]>(details);
        var criteria = new UserCriteria()
        {
            FIOs = keys,
            WithRemoved = getRemoved
        };
        return _userByCriteria.ExecuteQuery(criteria);
    }

    public IEnumerable<User> FromFirstNameAndLastName(ICollection<IUserDetails> details, bool getRemoved)
    {
        var keys = _mapper.Map<NameSurname[]>(details);
        var criteria = new UserCriteria()
        {
            NameSurnames = keys,
            WithRemoved = getRemoved
        };
        return _userByCriteria.ExecuteQuery(criteria);
    }

    public IEnumerable<User> FromLoginName(bool getRemoved, IEnumerable<string> logins)
    {
        var criteria = new UserCriteria()
        {
            Logins = logins,
            WithRemoved = getRemoved
        };
        return _userByCriteria.ExecuteQuery(criteria);
    }

    public IEnumerable<User> FromNumber(ICollection<IUserDetails> details, bool getRemoved)
    {
        var criteria = new UserCriteria()
        {
            Numbers = details.Select(x=>x.Number),
            WithRemoved = getRemoved
        };
        return _userByCriteria.ExecuteQuery(criteria);
    }

    public IEnumerable<User> FromSid(ICollection<IUserDetails> details, bool getRemoved)
    {
        var criteria = new UserCriteria()
        {
            SIDs = details.Select(x=>x.SID),
            WithRemoved = getRemoved
        };
        return _userByCriteria.ExecuteQuery(criteria);
    }

    public IEnumerable<User> FromExternalID(ICollection<IUserDetails> details, bool getRemoved)
    {
        var criteria = new UserCriteria()
        { 
            ExternalIDs= details.Select(x=>x.ExternalID),
            WithRemoved = getRemoved
        };
        return _userByCriteria.ExecuteQuery(criteria);
    }

    public IEnumerable<User> FromEmail(List<string> emails)
    {
        var criteria = new UserCriteria()
        {
            Emails = emails,
            WithRemoved = true
        };
        return _userByCriteria.ExecuteQuery(criteria);
    }

   
}