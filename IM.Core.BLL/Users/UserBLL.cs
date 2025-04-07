using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.BLL.Settings;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.Users;
using InfraManager.Linq;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using InfraManager.BLL.ServiceDesk;

namespace InfraManager.BLL.Users
{
    internal class UserBLL : IUserBLL, ISelfRegisteredService<IUserBLL>
    {
        private readonly IFindEntityByGlobalIdentifier<User> _finder;
        private readonly IDeputyUserBLL _deputyUsers;
        private readonly ISubdivisionFullNameQuery _subdivisionFullName;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _repository;
        private readonly IPagingQueryCreator _paging;
        private readonly IUserAccessBLL _userAccess;
        private readonly IUnitOfWork _saveChanges;
        private readonly IMemoryCache _cache;
        private readonly IUserQuery _userQuery;
        private readonly IUserColumnSettingsBLL _userColumnSettingsBLL;
        private readonly IColumnMapper<UserDetailsModel, UserForTable> _columnMapper;
        private readonly IObjectResponsibilityAccessBLL _objectResponsibilityAccessBLL;
        private readonly ICurrentUser _currentUser;
        private readonly ISystemUserGetter _systemUserGetter;
        private readonly IValidatePermissions<User> _permissionValidator;
        private readonly IReadonlyRepository<CustomControl> _customControlsRepository;
        private readonly IRepository<UserRole> _userRole;
        private readonly IRepository<GroupUser> _groupUser;
        private readonly IFindExecutorBLL<UserListItem, UserListFilter> _executorFinder;
        private readonly UserPasswordService _passwordService;
        private readonly IModifyEntityBLL<int, User, UserData, UserDetailsModel> _modifyEntityBLL;

        public UserBLL(
            IFindEntityByGlobalIdentifier<User> finder,
            IDeputyUserBLL deputyUsers,
            ISubdivisionFullNameQuery subdivisionFullName,
            IMapper mapper,
            IRepository<User> repository,
            IPagingQueryCreator paging,
            IUserAccessBLL userAccess,
            IUnitOfWork saveChanges,
            IMemoryCache cache,
            IUserQuery userQuery,
            IUserColumnSettingsBLL userColumnSettingsBLL,
            IColumnMapper<UserDetailsModel, UserForTable> columnMapper,
            ICurrentUser currentUser,
            ISystemUserGetter systemUserGetter,
            IObjectResponsibilityAccessBLL objectResponsibilityAccessBLL,
            IValidatePermissions<User> permissionValidator,
            IReadonlyRepository<CustomControl> customControlsRepository,
            IRepository<UserRole> userRole,
            IRepository<GroupUser> groupUsers,
            IFindExecutorBLL<UserListItem, UserListFilter> executorFinder,
            UserPasswordService passwordService,
            IModifyEntityBLL<int, User, UserData, UserDetailsModel> modifyEntityBLL)
        {
            _finder = finder;
            _deputyUsers = deputyUsers;
            _subdivisionFullName = subdivisionFullName;
            _mapper = mapper;
            _repository = repository;
            _paging = paging;
            _userAccess = userAccess;
            _saveChanges = saveChanges;
            _cache = cache;
            _userQuery = userQuery;
            _userColumnSettingsBLL = userColumnSettingsBLL;
            _columnMapper = columnMapper;
            _currentUser = currentUser;
            _objectResponsibilityAccessBLL = objectResponsibilityAccessBLL;
            _permissionValidator = permissionValidator;
            _systemUserGetter = systemUserGetter;
            _customControlsRepository = customControlsRepository;
            _userRole = userRole;
            _groupUser = groupUsers;
            _executorFinder = executorFinder;
            _passwordService = passwordService;
            _modifyEntityBLL = modifyEntityBLL;
        }

        public async Task<UserDetailsModel> DetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {

            var userDetailsModel = await _cache.GetOrCreateAsync(
                DetailsKey(id),
                async entry => await GetDetailsAsync(id, cancellationToken));
            var deputyList = new List<UserDetailsModel>();
            var deputyFilter = new DeputyUserListFilter
            {
                UserID = id,
                DeputyMode = DeputyMode.Deputy,
                Active = true,
                ShowFinished = true
            };

            foreach (var deputyUser in await _deputyUsers.GetDetailsArrayAsync(deputyFilter, cancellationToken))
            {
                var userDetails = await DetailsAsync(deputyUser.ChildUserID, cancellationToken);
                deputyList.Add(userDetails);
            }
            userDetailsModel.UserDeputyList = deputyList.ToArray();

            userDetailsModel.HasAdminRole = await _userAccess.HasAdminRoleAsync(id, cancellationToken);
            var grantedOperations = await _userAccess.GrantedOperationsAsync(id, cancellationToken);
            userDetailsModel.GrantedOperations = grantedOperations.ToArray();
            userDetailsModel.HasRoles = await _userAccess.HasRolesAsync(id, cancellationToken);
            userDetailsModel.Roles = (await _userAccess.GetRolesAsync(id, cancellationToken))
                .Select(role => _mapper.Map<LookupListItem<Guid>>(role))
                .ToArray();

            return userDetailsModel;
        }

        public async Task<UserDetailsModel> AnonymousDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetDetailsAsync(id, cancellationToken);
        }

        public async Task<UserDetailsModel> DetailsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await DetailsByExpression(x => x.Email == email, cancellationToken);
        }

        public async Task<UserDetailsModel> DetailsByLoginAsync(string login, CancellationToken cancellationToken = default)
        {
            return await DetailsByExpression(x => x.LoginName == login, cancellationToken);
        }

        public async Task<UserDetailsModel> DetailsByExpression(Expression<Func<User, bool>> expression, CancellationToken cancellationToken = default)
        {
            await _permissionValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.ViewDetails,
                cancellationToken);

            var user = await _repository.FirstOrDefaultAsync(expression, cancellationToken);
            if (user == null)
                return null;
            return await DetailsAsync(user.IMObjID, cancellationToken);
        }

        private async Task<UserDetailsModel> GetDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            User user;
            if (id == User.SystemUserGlobalIdentifier)
                user = await _systemUserGetter.GetAsync(cancellationToken) ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.User);
            else
                user = await _finder
                    .With(x => x.Position)
                    .With(x => x.Subdivision)
                        .ThenWith(x => x.Organization)
                    .With(x => x.Workplace)
                        .ThenWith(x => x.Room)
                            .ThenWith(x => x.Floor)
                                .ThenWith(x => x.Building)
                    .FindAsync(id, cancellationToken)
                        ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.User);

            var details = _mapper.Map<UserDetailsModel>(user);

            if (user.Subdivision != null)
            {
                details.SubdivisionFullName = await _subdivisionFullName.QueryAsync(user.Subdivision.ID, cancellationToken);
            }

            return details;
        }

        public static string DetailsKey(Guid userId) => $"UserDetails_{userId}";

        //TODO: Переписать, используя стандартные сервисы
        public async Task<UserListItem[]> ListAsync(UserListFilter filterBy, CancellationToken cancellationToken)
        {
            await _permissionValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.ViewDetails,
                cancellationToken);

            Expression<Func<User, bool>> predicate = user => true;

            if (!string.IsNullOrWhiteSpace(filterBy.SearchRequest))
            {
                predicate = user => (user.Surname.ToLower() + " " + user.Name.ToLower() + " " + user.Patronymic.ToLower())
                    .Contains(filterBy.SearchRequest.ToLower());
            }//поставить индекс а не toLower

            if (filterBy.SubdivisionID.HasValue)
            {
                predicate = predicate.And(user => user.Subdivision.ID == filterBy.SubdivisionID.Value);
            }

            if (filterBy.SubdivisionIDList != null  && filterBy.SubdivisionIDList.Any())
            {
                predicate = predicate.And(user => filterBy.SubdivisionIDList.Contains(user.Subdivision.ID));
            }

            if (filterBy.OrganizationID.HasValue)
            {
                predicate = predicate.And(user => user.Subdivision.Organization.ID == filterBy.OrganizationID.Value);
            }

            if (filterBy.ControlsObjectID.HasValue)
            {
                var customControlsQuery = _customControlsRepository.Query();
                Expression<Func<User, bool>> hasCustomControlExpression =
                    u => customControlsQuery.Any(
                        x => x.UserId == u.IMObjID
                            && x.ObjectId == filterBy.ControlsObjectID
                            && x.ObjectClass == filterBy.ControlsObjectClassID);
                predicate = predicate.And(filterBy.ControlsObjectValue ? hasCustomControlExpression : hasCustomControlExpression.Not());
            }

            if (filterBy.RoleID.HasValue)
            {
                var userRoleQuery = _userRole.Query();
                predicate = predicate.And(u => userRoleQuery.Any(ur => ur.UserID == u.IMObjID && ur.RoleID == filterBy.RoleID.Value));
            }

            if (filterBy.GroupID.HasValue)
            {
                var userGroupQuery = _groupUser.Query();
                predicate = predicate.And(u => userGroupQuery.Any(ur => ur.UserID == u.IMObjID && ur.GroupID == filterBy.GroupID.Value));
            }

            if (filterBy.PositionIDGuid.HasValue)
            {
                predicate = predicate.And(user => user.Position.IMObjID == filterBy.PositionIDGuid.Value);
            }

            if (filterBy.PositionIDInt != null)
            {
                predicate = predicate.And(user => user.Position.ID == filterBy.PositionIDInt.Value);
            }

            if (filterBy.UserID.HasValue)
            {
                predicate = predicate.And(user => user.IMObjID == filterBy.UserID.Value);
            }
            
            if (filterBy.UserIDList != null && filterBy.UserIDList.Any())
            {
                predicate = predicate.And(user => filterBy.UserIDList.Contains(user.IMObjID));
            }

            if (filterBy.OnlyKBExpert)
            {
                predicate = predicate.And(User.HasOperation(OperationID.BeKnowledgeBaseExpert));
            }
          
            IEnumerable<User> users;

            if (filterBy != null)
            {
                var query = _repository.Query().Where(predicate).OrderBy(x => x.ID); //Мб сортировать по каким то критериям, которые увидит пользователь? например фамилия и тд.
                var paging = _paging.Create(query);

                users = await paging
                    .PageAsync(filterBy.StartRecordIndex, filterBy.CountRecords, cancellationToken);
            }
            else
            {
                users = await _repository.ToArrayAsync(predicate, cancellationToken);
            }

            return users.Select(u => _mapper.Map<UserListItem>(u)).ToArray();
        }

        public async Task<UserDetailsModel> GetSignInUserAsync(string login, string password, bool needValidatePassword,
            CancellationToken cancellationToken = default)
        {
            var user = await _repository
                           .With(x => x.Subdivision)
                           .With(x => x.Workplace)
                           .ThenWith(x => x.Room)
                           .ThenWith(x => x.Floor)
                           .ThenWith(x => x.Building)
                           .FirstOrDefaultAsync(x => x.LoginName.ToLower() == login.ToLower(), cancellationToken)
                       ?? throw new ObjectNotFoundException($"Отсутствует пользователь с именем {login}.");

            if (user.Removed)
            {
                throw new AccessDeniedException("User is removed");
            }
            
            if (needValidatePassword) //TODO пересмотреть этот вариант
                if (!_passwordService.ValidateLoginPassword(user.SDWebPassword, password))
                    throw new ObjectNotFoundException($"Неверный пароль пользователя с именем {user.LoginName}");

            var model = _mapper.Map<UserDetailsModel>(user);

            if (user.Subdivision != null)
                model.SubdivisionFullName =
                    await _subdivisionFullName.QueryAsync(user.Subdivision.ID, cancellationToken);

            model.HasAdminRole = await _userAccess.HasAdminRoleAsync(user.IMObjID, cancellationToken);
            var grantedOperations = await _userAccess.GrantedOperationsAsync(user.IMObjID, cancellationToken);
            model.GrantedOperations = grantedOperations.ToArray();
            model.HasRoles = await _userAccess.HasRolesAsync(user.IMObjID, cancellationToken);

            return model;
        }

        public async Task<UserListItem[]> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            await _permissionValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.ViewDetails,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentOutOfRangeException($"email can not be empty");
            }

            var users = await _repository.ToArrayAsync(x => x.Email == email, cancellationToken);

            return users.Select(u => _mapper.Map<UserListItem>(u)).ToArray();
        }

        public async Task<UserEmailDetails[]> GetEmailsAsync(BaseFilter filter, CancellationToken cancellationToken)
        {
            await _permissionValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.ViewDetails,
                cancellationToken);

            var query = _repository.Query().Where(x => !string.IsNullOrEmpty(x.Email));

            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(u => u.Email.ToLower().Contains(filter.SearchString.ToLower()));
            }

            var paging = _paging.Create(query.OrderBy(x => x.Email));

            var emails = await paging
                .PageAsync(filter.StartRecordIndex, filter.CountRecords, cancellationToken);

            return _mapper.Map<UserEmailDetails[]>(emails);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _permissionValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.Delete,
                cancellationToken);

            if (Guid.Empty == id) throw new ArgumentOutOfRangeException("Guid can not be empty");

            var entity = await _repository.FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken)
                    ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.User);

            using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
            {
                _repository.Delete(entity);
                await _objectResponsibilityAccessBLL.DeleteByOwnerAsync(id, cancellationToken);
                await _saveChanges.SaveAsync(cancellationToken);
                transaction.Complete();
            }
        }

        public async Task CreateAsync(UserData userData, CancellationToken cancellationToken)
        {
            await _permissionValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.Insert,
                cancellationToken);

            var user = new User(userData.Name, userData.Surname);
            _mapper.Map(userData, user);

            _repository.Insert(user);
            await _saveChanges.SaveAsync(cancellationToken);
        }

        public async Task UpdateAsync(Guid id, UserData userData, CancellationToken cancellationToken) //TODO guid => int
        {
            var user = await _repository.FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken) ??
                       throw new ObjectNotFoundException("User was not found"); 
            
            await _modifyEntityBLL.ModifyAsync(user.ID, userData, cancellationToken);
            await _saveChanges.SaveAsync(cancellationToken);

            _cache.Remove(DetailsKey(id));
        }

        public async Task<UserDetailsModel[]> GetTableAsync(UserFilter filter, CancellationToken cancellationToken)
        {
            await _permissionValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.ViewDetailsArray,
                cancellationToken);

            var columns = await _userColumnSettingsBLL.GetAsync(_currentUser.UserId, filter.ViewName, cancellationToken);
            var orderColumn = columns.GetSortColumn();
            orderColumn.PropertyName = _columnMapper.MapFirst(orderColumn.PropertyName);

            var operationIDs = GetOperationsByFilter(filter);

            var users = await _userQuery.ExecuteAsync(filter.ID,
                operationIDs,
                filter.ClassID, filter.SearchString,
                orderColumn,
                filter.CountRecords,
                filter.StartRecordIndex,
                filter.WithEmails,
                kbExpert: filter.OnlyKBExpert,
                cancellationToken);

            return _mapper.Map<UserDetailsModel[]>(users);
        }

        private OperationID[] GetOperationsByFilter(UserFilter filter)
        {
            var operationIDs = new List<OperationID>();
            if (filter.ExecutorOnly)
                operationIDs.Add(OperationID.SD_General_Executor);
            if (filter.MateriallyResponsibleOnly)
                operationIDs.Add(OperationID.MateriallyResponsible);
            if (filter.OwnerOnly)
                operationIDs.Add(OperationID.SD_General_Owner);
            if (filter.IsSupportAdministrator)
                operationIDs.Add(OperationID.SD_General_Administrator);
            if (filter.OnlyParticipantsAgreement)
                operationIDs.Add(OperationID.SD_General_VotingUser);

            return operationIDs.ToArray();
        }

        public async Task<UserDetailsModel[]> ListDetailsAsync(UserListFilter filterBy, CancellationToken cancellationToken = default)
        {
            UserListItem[] usersList;

            if (filterBy.SDExecutor)
            {
                usersList = await _executorFinder.FindAsync(filterBy, cancellationToken);
            }
            else
            {
                if (filterBy.GetSubdivisionByUserIDList)
                {
                    var subdivisionsID = (await ListAsync(new UserListFilter { UserIDList = filterBy.UserIDList }, cancellationToken)).Select(x => x.SubdivisionID.Value).Distinct().ToList();
                    filterBy = new UserListFilter
                    {
                        RoleID = filterBy.RoleID,
                        GroupID = filterBy.GroupID,
                        ControlsObjectClassID = filterBy.ControlsObjectClassID,
                        ControlsObjectID = filterBy.ControlsObjectID,
                        ControlsObjectValue = filterBy.ControlsObjectValue,
                        CountRecords = filterBy.CountRecords,
                        GetSubdivisionByUserIDList = false,
                        OrganizationID = filterBy.OrganizationID,
                        PositionIDGuid = filterBy.PositionIDGuid,
                        PositionIDInt = filterBy.PositionIDInt,
                        SearchRequest = filterBy.SearchRequest,
                        StartRecordIndex = filterBy.StartRecordIndex,
                        SubdivisionID = filterBy.SubdivisionID,
                        SubdivisionIDList = subdivisionsID,
                        UserID = filterBy.UserID,
                        UserIDList = null
                    };
                }
                usersList = await ListAsync(filterBy, cancellationToken);
            }
 
            return await EnrichWithDetailsAsync(usersList, cancellationToken);
        }

        private async Task<UserDetailsModel[]> EnrichWithDetailsAsync(UserListItem[] userList, CancellationToken cancellationToken)
        {
            var result = new List<UserDetailsModel>(userList.Length);
            foreach (var user in userList)
            {
                var details = await DetailsAsync(user.IMObjID, cancellationToken);
                if (details != null)
                {
                    result.Add(details);
                }
            }

            return result.ToArray();
        }
    }
}
