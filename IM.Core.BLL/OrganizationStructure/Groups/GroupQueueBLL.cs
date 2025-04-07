using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.Asset;
using InfraManager.BLL.ServiceDesk;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure.Groups
{
    // TODO разделить логику по группам и членами группы(performers)
    internal class GroupQueueBLL : IGroupQueueBLL, ISelfRegisteredService<IGroupQueueBLL>
    {
        private readonly IMapper _mapper;
        private readonly IGuidePaggingFacade<Group, GroupQueueForTable> _groupList;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<GroupUser> _groupUserRepository;
        private readonly IReadonlyRepository<User> _repositoryUsers;
        private readonly IFindExecutorBLL<GroupDetails, GroupFilter> _executorFinder;

        public GroupQueueBLL(
            IMapper mapper,
            IGuidePaggingFacade<Group, GroupQueueForTable> groupList,
            IRepository<Group> repository,
            IRepository<GroupUser> groupUser,
            IReadonlyRepository<User> repositoryUsers,
            IFindExecutorBLL<GroupDetails, GroupFilter> executorFinder)
        {
            _mapper = mapper;
            _groupList = groupList;
            _groupRepository = repository;
            _groupUserRepository = groupUser;
            _repositoryUsers = repositoryUsers;
            _executorFinder = executorFinder;
        }

        public async Task<GroupDetails[]> GetListAsync(GroupFilter filter,
            CancellationToken cancellationToken = default)
        {
            if (filter.SDExecutor)
            {
                return await _executorFinder.FindAsync(filter, cancellationToken);
            }

            var query = _groupRepository.DisableTrackingForQuery().With(x => x.ResponsibleUser)
                .WithMany(x => x.QueueUsers).ThenWith(x => x.User).Query().Where(Group.IsNotNullObject);

            if (filter.IsNeedValidateType)
            {
                query = filter.IsAll
                    ? query.Where(x => x.Type != GroupType.None)
                    : query.Where(x =>
                        x.Type == GroupType.All ||
                        GroupTypeExtensions.GetPossibleValues(filter.GetGroupTypeData).Contains((byte)x.Type));
            }
   
            var result = await _groupList.GetPaggingAsync(
                filter,
                query,
                x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
                cancellationToken
            );
            
            return _mapper.Map<GroupDetails[]>(result);
        }

        #region Исполнители
        public async Task<GroupQueueUserDetails> GetPerformerByIdAsync(Guid userID, Guid groupID, CancellationToken cancellationToken = default)
        {
            await ValidateQueueAsync(groupID, cancellationToken);
            await ValidateGroupUserAsync(userID, groupID, cancellationToken);

            var dataUser = await _repositoryUsers.FirstOrDefaultAsync(c => c.IMObjID == userID, cancellationToken);
            return _mapper.Map<GroupQueueUserDetails>(dataUser);
        }

        public async Task<GroupQueueUserDetails[]> GetPerformersAsync(Guid groupID, bool isPerformers, string searchName, CancellationToken cancellationToken)
        {
            await ValidateQueueAsync(groupID, cancellationToken);

            var groupUsers = await _groupUserRepository.ToArrayAsync(x => x.GroupID == groupID, cancellationToken);
            var userIDs = groupUsers.Select(x => x.UserID).ToArray();

            var query = _repositoryUsers.Query();

            query = isPerformers 
                ? query.Where(user => userIDs.Contains(user.IMObjID))
                : query.Where(user => !userIDs.Contains(user.IMObjID));

            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(c => c.FullName.Contains(searchName));
            }

            var performers = await query.ExecuteAsync(cancellationToken);
            return _mapper.Map<GroupQueueUserDetails[]>(performers);
        }

        private async Task ValidateQueueAsync(Guid queueID, CancellationToken cancellationToken = default)
        {
            var queue = await _groupRepository.AnyAsync(x => x.IMObjID == queueID, cancellationToken);
            if (!queue)
            {
                throw new ObjectNotFoundException<Guid>(queueID, "Queue not found");
            }
        }

        private async Task ValidateGroupUserAsync(Guid groupUserID, Guid queueID, CancellationToken cancellationToken = default)
        {
            var groupUser = await _groupUserRepository.AnyAsync(x => x.GroupID == queueID && x.UserID == groupUserID, cancellationToken);
            if (!groupUser)
            {
                throw new ObjectNotFoundException(
                    $"GroupQueueUser not found; GroupQueueUserID = {groupUserID} && QueueID = {queueID} ");
            }
        }
        #endregion
    }
}
