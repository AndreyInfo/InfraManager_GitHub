using AutoMapper;
using IM.Core.Import.BLL.Interface.OrganizationStructure.Users;
using InfraManager;
using InfraManager.DAL;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Debug;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.Log;
using InfraManager.DAL.Users;
using Microsoft.Extensions.Configuration;

namespace IM.Core.Import.BLL.OrganizationStructure.Users;

public class UsersBLL : IUsersBLL, ISelfRegisteredService<IUsersBLL>
{
    private readonly IRepository<User> _repository;
    private readonly IUnitOfWork _saveChanges;
    private readonly ILocalLogger<UsersBLL> _logger;
    private readonly IBaseImportMapper<IUserDetails, User> _importMapper;
    private readonly IConfiguration _configuration;
    private readonly IAllUserQuery _allUser;

    private IMapper _mapper;

    public UsersBLL(
        IRepository<User> repository,
        IUnitOfWork saveChanges,
        ILocalLogger<UsersBLL> logger,
        IMapper mapper, IBaseImportMapper<IUserDetails, User> importMapper,
        IConfiguration configuration, 
        IAllUserQuery allUser)
    {
        _repository = repository;
        _saveChanges = saveChanges;
        _logger = logger;
        _mapper = mapper;
        _importMapper = importMapper;
        _configuration = configuration;
        _allUser = allUser;
    }

    public async Task<int> CreateUsersAsync(IEnumerable<User> users, CancellationToken cancellationToken = default)
    {
        try
        {
            var count = GetBlockCount();
            var i = 0;
            int created = 0;
            var blocks = users.GroupBy(x => i++ / count);
            foreach (var block in blocks)
            {
                try
                {
                    foreach (var entity in block)
                    {
                        _repository.Insert(entity);
                    }

                    await _saveChanges.SaveAsync(cancellationToken);
                    created += block.Count();
                    _logger.Information($"Создано {created} пользователей");
                }
                catch (Exception e)
                {
                    foreach (var element in block)
                    {
                        var debug = _mapper.Map<UserDebugData>(element);
                        _logger.Error("Ошибка в блоке создания пользователя",e.ToString(), e);
                        _logger.Verbose(debug.ToString());
                    }
                }
            }

            return created;
        }
        catch (Exception e)
        {
            _logger.Error("Ошибка создания пользователей", string.Empty,e);
            throw;
        }
    }

    public async Task<int> UpdateUsersAsync(Dictionary<IUserDetails, User> updateUsers,
        ImportData<IUserDetails, User> data,
        CancellationToken cancellationToken = default)
    {
        try
        {
            
            var pairs = updateUsers
                .Select(x=>(x.Key,x.Value));
            
            var count = GetBlockCount();

            int i = 0;
            int updated = 0;
            var groupedResults = pairs.GroupBy(x => i++ / count);
            foreach (var block in groupedResults)
            {
                List<UserDetails> sourceBlock= default;
                try
                {
                    sourceBlock = _mapper.ProjectTo<UserDetails>(block.Select(x=>x.Value).AsQueryable()).ToList();
                    _importMapper.Map(data, block);
                    await _saveChanges.SaveAsync(cancellationToken);
                    updated += block.Count();
                    _logger.Information($"Обновлено {updated} пользователей");

                }
                catch (Exception e)
                {
                    foreach (var pair in block)
                    {
                        var userDbgData = _mapper.Map<UserDebugData>(pair.Value);
                        _logger.Error("Ошибка загрузки блока", string.Empty, e);
                        _logger.Verbose(userDbgData.ToString());
                        if (sourceBlock != default)
                            foreach (var backupData in sourceBlock.Zip(block))
                            {
                                _mapper.Map(backupData.First, backupData.Second.Value);
                            }
                    }
                }
            }

            return updated;
        }
        catch (Exception e)
        {
            _logger.Error("Ошибка обновления пользователей", string.Empty, e);
            throw;
        }
    }

    private int GetBlockCount()
    {
        var countString = _configuration["BlockSaveCount"];

        if (!int.TryParse(countString, out var count) || count <= 0)
            throw new InvalidCastException("BlockSaveCount должен быть целым числом больше нуля");
        return count;
    }

    private static IEnumerable<(IUserDetails, User)> GetPairs(Dictionary<IUserDetails, User> updateUsers)
    {
        foreach (var updateUser in updateUsers)
        {
            if (!updateUser.Value.Removed)
            {
                yield return (updateUser.Key, updateUser.Value);
            }
            else
            {
                //todo:Добавить в лог данные об ошибке, хотя поидее сюда никогда не должно попасть
            }
        }
    }

    public void InitUserForUpdate(User user, bool restoreRemoved)
    {
        if (user.Removed && restoreRemoved)
        {
            user.Removed = false;
        }
    }

    public async Task<IEnumerable<User>> GetByFullNameAsync(List<IUserDetails> users, CancellationToken cancellationToken = default)
    {
        var usersNames = users.Select(x => x.FullName);
        return _repository.Where(x => usersNames.Contains(x.FullName));
    }

    public async Task<IEnumerable<User>> GetByFirstAndLastNameAsync(List<IUserDetails> users, CancellationToken cancellationToken = default)
    {
        var usersNames = users.Select(x=>x.Name).Distinct();
        var usersSurname = users.Select(x => x.Surname).Distinct();
        var resultUsersSurnames = await _repository.ToArrayAsync(x => usersSurname.Any(y=>y == x.Surname ));
        var resultUsersNames = await _repository.ToArrayAsync(x => usersNames.Any(y => y == x.Name));

        List<User> resultUsers = new List<User>();

        foreach (var user in resultUsersSurnames)
        {
            if (resultUsersNames.Any(x => x.Name == user.Name && x.Surname == user.Surname))
            { 
                resultUsers.Add(user);
            }
        }

        return resultUsers;
    }

    
    public async Task<IEnumerable<User>> GetByNumberAsync(List<IUserDetails> users, CancellationToken cancellationToken = default)
    {
        var usersNumbers = users.Select(x => x.Number);
        return await _repository.ToArrayAsync(x => usersNumbers.Contains(x.Number), cancellationToken);
    }

    public async Task<IEnumerable<User>> GetByLoginAsync(List<IUserDetails> users, CancellationToken cancellationToken = default)
    {
        var usersLogins = users.Select(x => x.LoginName);
        return await _repository.ToArrayAsync(x => usersLogins.Contains(x.LoginName), cancellationToken);
    }

    public async Task<IEnumerable<User>> GetBySIDNameAsync(List<IUserDetails> users, CancellationToken cancellationToken = default)
    {
        var usersSIDs = users.Select(x => x.SID);
        return await _repository.ToArrayAsync(x => usersSIDs.Contains(x.SID), cancellationToken);
    }

    public async Task<IEnumerable<User>> GetByExternalIDAsync(List<IUserDetails> users, CancellationToken cancellationToken = default)
    {
        var usersExternalIDs = users.Select(x => x.ExternalID);
        return await _repository.ToArrayAsync(x => usersExternalIDs.Contains(x.ExternalID), cancellationToken);
    }

}