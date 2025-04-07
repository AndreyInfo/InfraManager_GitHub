using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.OrganizationStructure.Users;
using InfraManager;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.OrganizationStructure;

public class UserImportConnector : IEntityImportConnector<IUserDetails, User>,
    ISelfRegisteredService<IEntityImportConnector<IUserDetails, User>>
{
    private readonly IUsersBLL _usersBLL;
    private readonly IBaseImportMapper<IUserDetails, User> _userImportMapper;
    private readonly IImportConnectorBase<IUserDetails, User> _userImportConnectorBase;
    private readonly IAdditionalParametersForSelect? _additional;
    private readonly ILocalLogger<UserImportConnector> _logger;

    public UserImportConnector(IUsersBLL usersBLL,
        IBaseImportMapper<IUserDetails, User> userImportMapper,
        IImportConnectorBase<IUserDetails, User> userImportConnectorBase,
        IAdditionalParametersForSelect? additional,
        ILocalLogger<UserImportConnector> logger, 
        IBaseImportMapper<IUserDetails, User> importMapper)
    {
        _usersBLL = usersBLL;
        _userImportMapper = userImportMapper;
        _userImportConnectorBase = userImportConnectorBase;
        _additional = additional;
        _logger = logger;
    }

    public async Task SaveOrUpdateEntitiesAsync(ICollection<IUserDetails> users,
        IAdditionalParametersForSelect? additional, ImportData<IUserDetails, User> data,
        ErrorStatistics<IUserDetails> errorEntries,
        CancellationToken cancellationToken = default)
    {
        _userImportConnectorBase.AfterCheckForErrors(users, errorEntries, data);
        var lookup = users.ToLookup(data.DetailsKey);

        var notEmptyData = (List<Queue<IUserDetails>>?) lookup
            .Select(group => new Queue<IUserDetails>(group)).ToList();

        do
        {
            var uniqueUsers = notEmptyData
                .Select(x => x.Dequeue()).ToList();

            _logger.Information("Загрузка пользователей");

            await ProcessUsersAsync(uniqueUsers, data, errorEntries, cancellationToken);
            
            notEmptyData = notEmptyData.Where(x => x.Any()).ToList();
        } while (notEmptyData.Any());
    }

    public void LogStatistics(ErrorStatistics<IUserDetails> errorEntities,
        ImportData<IUserDetails, User> importData)
    {
        _logger.Information($"Всего создано пользователей: {errorEntities.CreateCount}");
        _logger.Information($"Всего обновлено пользователей: {errorEntities.UpdateCount}");
        var errorCount = errorEntities.ErrorDetails.Count;
        _logger.Information(
            $"Пользователей с ошибками: {errorCount + errorEntities.UpdatedErrors + errorEntities.CreatedErrors}. При обновлении: {errorEntities.UpdatedErrors}, при добавлении: {errorEntities.CreatedErrors}.");
        SetDetailEntitiesErrorsToLog(errorEntities.ErrorDetails, importData);
    }

    private async Task ProcessUsersAsync(ICollection<IUserDetails> users,
        ImportData<IUserDetails, User> data,
        ErrorStatistics<IUserDetails> errorEntities,
        CancellationToken cancellationToken)
    {
        _logger.Information("Загрузка информации о существующих пользователях");

        var usersFromBase = (await data.Func(users, _additional, cancellationToken)).ToList();

        _logger.Information("Фильтрация пользователей");

        ValidateUsers(users, errorEntities, data);
        //перевод в ошибку пользователей, соответствующих дубликатам в базе

        _logger.Information("Поиск существующих пользователей");

        var updateUsers = await _userImportConnectorBase.GetModelsForUpdateAsync(usersFromBase, users, errorEntities,
            data, null, null, cancellationToken);
        _logger.Information("Поиск пользователей для обновления");


        var createUsers = await _userImportConnectorBase.GetModelsForCreateAsync(data, users, updateUsers.FoundModels,
            errorEntities, _userImportMapper, null, (users, error, token) => AdditionalValidationFunc(users, error, data, token), cancellationToken);


        if (updateUsers.ModelsForUpdate.Count > 0)
        {
            var updatedUsersCount = await _usersBLL.UpdateUsersAsync(updateUsers.ModelsForUpdate, data, cancellationToken);
            errorEntities.UpdateCount += updatedUsersCount;
            errorEntities.UpdatedErrors += updateUsers.ModelsForUpdate.Count - updatedUsersCount;
        }

        if (createUsers.Count > 0)
        {
            _logger.Information("Загрузка новых пользователей");

            
            foreach (var user in createUsers.Values)
            {
                user.WorkplaceID ??= ImportConstants.DefaultUserWorkplaceID;
                user.RoomID ??= ImportConstants.DefaultUserRoomID;
            }

            
            _userImportConnectorBase.CheckBeforeCreate(createUsers, errorEntities, data);

            var createdUsersCount = await _usersBLL.CreateUsersAsync(createUsers.Values, cancellationToken);
            errorEntities.CreateCount += createdUsersCount;
            errorEntities.CreatedErrors += createUsers.Count - createdUsersCount;
        }
    }

    private async Task AdditionalValidationFunc(ICollection<IUserDetails> forCreate,
        ErrorStatistics<IUserDetails> errorUsers, ImportData<IUserDetails, User> data, CancellationToken cancellationToken)
    {
        var duplicates = await data.GetDuplicateKeysInBaseAsync(forCreate, _additional, cancellationToken);
        foreach (var userDetail in duplicates)
        {
            errorUsers.ErrorDetails.Add(userDetail.Detail, userDetail.Message);
            forCreate.Remove(userDetail.Item1);
        }
    }

    private void SetDetailEntitiesErrorsToLog(ErrorDetails<IUserDetails> errors,
        ImportData<IUserDetails, User> importData)
    {
        if (errors.Count > 0)
        {
            foreach (var error in errors.GetMessages(x=>LogHelper.ToOutputFormat(importData.DetailsKey(x)?.ToString())))
            {
                var errorFullName = LogHelper.ToOutputFormat(error);
                _logger.Information($"Ошибка при обновлении пользователя {errorFullName}");
            }
        }
    }


    private void ValidateUsers(ICollection<IUserDetails> importUsers, ErrorStatistics<IUserDetails> errorUsers,
        ImportData<IUserDetails, User> data)
    {
        var notValidated = (
            from current in importUsers
            where data.ValidateAfter(current)
                 
            select current
        ).ToList();
        
        var noSubdivision = (
            from current in importUsers
            where  !current.SubdivisionID.HasValue
            select current
        ).ToList();
        

        _userImportConnectorBase.MoveUsersToError(importUsers, errorUsers, notValidated, "Недостаточно данных для импорта");
        _userImportConnectorBase.MoveUsersToError(importUsers, errorUsers, noSubdivision, "Не установлено подраздклкение");
    }
}