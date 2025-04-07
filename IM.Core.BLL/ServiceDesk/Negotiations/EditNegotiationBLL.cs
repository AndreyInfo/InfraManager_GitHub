using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Negotiations;
using InfraManager.DAL.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal abstract class EditNegotiationBLL<TParent>: IEditNegotiationBLL 
        where TParent : class, ICreateNegotiation
    {
        #region .ctor

        private readonly IRepository<Negotiation> _repository;
        private readonly IFinder<Negotiation> _finder;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBuildObject<NegotiationDetails, Negotiation> _detailsBuilder;
        private readonly IServiceMapper<NegotiationMode, ICalculateNegotiationStatus> _statusCalculators;
        private readonly IMapper _mapper;
        private readonly IReadonlyRepository<User> _usersRepository;
        private readonly IValidateObjectPermissions<Guid, Negotiation> _permissionsValidator;
        private readonly IRemoveEntityBLL<Guid, Negotiation> _removeService;
        private readonly IBuildSpecification<Guid, Guid> _userIsDeputySpecificationBuilder;

        protected EditNegotiationBLL(
            IRepository<Negotiation> repository,
            IFinder<Negotiation> finder,
            IUnitOfWork unitOfWork,
            IBuildObject<NegotiationDetails, Negotiation> detailsBuilder,
            IServiceMapper<NegotiationMode, ICalculateNegotiationStatus> statusCalculators,
            IMapper mapper,
            IReadonlyRepository<User> usersRepository,
            IValidateObjectPermissions<Guid, Negotiation> permissionsValidator,
            IRemoveEntityBLL<Guid, Negotiation> removeService,
            IBuildUserIsDeputySpecification userIsDeputySpecificationBuilder,
            ICurrentUser currentUser,
            ILogger logger,
            IValidatePermissions<TParent> parentPermissionsValidator)
        {
            _repository = repository;
            _finder = finder;
            _unitOfWork = unitOfWork;
            _detailsBuilder = detailsBuilder;
            _statusCalculators = statusCalculators;
            _mapper = mapper;
            _usersRepository = usersRepository;
            _permissionsValidator = permissionsValidator;
            _removeService = removeService;
            _userIsDeputySpecificationBuilder = userIsDeputySpecificationBuilder;
            CurrentUser = currentUser;
            Logger = logger;
            ParentPermissionsValidator = parentPermissionsValidator;
        }

        protected ICurrentUser CurrentUser { get; }
        protected ILogger Logger { get; }
        protected IValidatePermissions<TParent> ParentPermissionsValidator { get; }

        #endregion

        #region TParent permissions

        private async Task ValidateParentEditPermissionsAsync(Guid flowID, CancellationToken cancellationToken = default)
        {
            if (!await ParentPermissionsValidator.UserHasPermissionAsync(CurrentUser.UserId, ObjectAction.Update, cancellationToken))
            {
                throw new AccessDeniedException($"Edit {typeof(TParent).Name}");
            }
            Logger.LogTrace($"FlowID: {flowID}. Permissions to edit {typeof(TParent).Name} granted.");
        }

        protected abstract Task ValidateParentObjectAccessAsync(Guid flowID, Guid parentObjectID, CancellationToken cancellationToken = default);

        protected abstract Task<TParent> FindParentObjectAsync(Guid flowID, Guid parentObjectID, CancellationToken cancellationToken = default);

        #endregion

        #region Insert, Update, Delete Negotiation

        public async Task<NegotiationDetails> AddAsync(Guid parentObjectID, NegotiationData data, CancellationToken cancellationToken = default)
        {
            var flowID = Guid.NewGuid();
            Logger.LogTrace($"FlowID: {flowID}. User (ID = {CurrentUser.UserId}) is creating negotiation for {typeof(TParent).Name} (IMObjID = {parentObjectID})");
            await ValidateParentObjectAccessAndEditPermissionsAsync(flowID, parentObjectID, cancellationToken);
            var negotiationObject = await FindParentObjectAsync(flowID, parentObjectID, cancellationToken);

            var negotiation = negotiationObject.CreateNegotiation();

            await ModifyNegotiationAsync(
                negotiationObject,
                negotiation,
                data,
                cancellationToken);

            _repository.Insert(negotiation);
            await _unitOfWork.SaveAsync(cancellationToken);
            Logger.LogInformation($"FlowID: {flowID}. Negotiation (ID = {negotiation.IMObjID}) is successfully created by user (ID = {CurrentUser.UserId}).");

            return await _detailsBuilder.BuildAsync(negotiation, cancellationToken);
        }

        public async Task<NegotiationDetails> UpdateAsync(Guid id, NegotiationData data, CancellationToken cancellationToken = default)
        {
            var flowID = Guid.NewGuid();
            Logger.LogTrace($"FlowID: {flowID}. User (ID = {CurrentUser.UserId}) is updating negotiation (ID = {id})");

            var negotiation = await FindAsync(_finder.WithMany(x => x.NegotiationUsers), flowID, id, cancellationToken);
            if (data.ModifyNegotiation // Меняют тип, название согласования или меняют состав голосующих 
                || !await _permissionsValidator.ObjectIsAvailableAsync(CurrentUser.UserId, id, cancellationToken)
                || !negotiation.NegotiationUsers.Any()
                || !(Negotiation.UserIsNegotiator(CurrentUser.UserId).IsSatisfiedBy(negotiation) 
                    || negotiation.NegotiationUsers.All(x => _userIsDeputySpecificationBuilder.Build(CurrentUser.UserId).IsSatisfiedBy(x.UserID))))
            {
                // тогда пользователь должен иметь права на редактирование родительского объекта
                await ValidateParentObjectAccessAndEditPermissionsAsync(flowID, negotiation.ObjectID, cancellationToken);
            }

            if (negotiation.IsFinished)
            {
                throw new ObjectReadonlyException(
                    new InframanagerObject(negotiation.IMObjID, ObjectClass.Negotiation));
            }

            var negotiationObject = await FindParentObjectAsync(flowID, negotiation.ObjectID, cancellationToken);

            await ModifyNegotiationAsync(
                negotiationObject,
                negotiation,
                data,
                cancellationToken);

            if (negotiation.Status != NegotiationStatus.Created)
            {
                negotiation.Status = _statusCalculators
                    .Map(negotiation.Mode)
                    .Calculate(negotiation.NegotiationUsers); //TODO: Отрефакторить клиент, удалять / добавлять пользователей на лету, кнопку Save убрать
                //TODO: Когда будет сервис очереди сообщений, отправить всем email-ы
            }
            await _unitOfWork.SaveAsync(cancellationToken);
            Logger.LogInformation($"FlowID: {flowID}. Negotiation (ID = {id}) is modified by user (ID = {CurrentUser.UserId}).");

            return await _detailsBuilder.BuildAsync(negotiation, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var flowID = Guid.NewGuid();
            Logger.LogTrace($"FlowID: {flowID}. User (ID = {CurrentUser.UserId}) is trying to remove negotiation (ID = {id}).");

            var negotiation = await FindAsync(_finder, flowID, id, cancellationToken);
            await ValidateParentObjectAccessAndEditPermissionsAsync(flowID, negotiation.ObjectID, cancellationToken);
            var negotiationObject = await FindParentObjectAsync(flowID, negotiation.ObjectID, cancellationToken);            
            negotiationObject.UtcDateModified = DateTime.UtcNow;
            await _removeService.RemoveAsync(id, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            Logger.LogInformation($"FlowID: {flowID}. Negotiation (id = {id}) is successfully deleted by user (ID = {CurrentUser.UserId}).");
        }

        private async Task ModifyNegotiationAsync(
            ICreateNegotiation negotiationObject,
            Negotiation negotiation,
            NegotiationData data,
            CancellationToken cancellationToken = default)
        {
            negotiationObject.UtcDateModified = DateTime.UtcNow; // Связанный объект должен быть помечен как измененный (со всеми вытекающими ивентами)

            if (negotiation.Status == NegotiationStatus.Created) // В другом состоянии изменение атрибутов невозмоожно
            {
                _mapper.Map(data, negotiation);
                if (data.IsStarted)
                {
                    negotiation.Start();
                }
            }

            if (data.UserIDs == null || !data.UserIDs.Any())
            {
                return;
            }

            var users = await _usersRepository.ToArrayAsync(u => data.UserIDs.Contains(u.IMObjID), cancellationToken);

            foreach (var user
                in users.Where(u => !negotiation.NegotiationUsers.Any(x => x.UserID == u.IMObjID)))
            {
                negotiation.AddUser(user);
            }

            var usersToDelete = negotiation.NegotiationUsers
                .Where(x => !users.Any(u => u.IMObjID == x.UserID))
                .Where(x => x.VotingType == VotingType.None) // не дадим удалить проголосовавшего
                .ToArray();

            foreach (var user in usersToDelete)
            {
                negotiation.NegotiationUsers.Remove(user);
            }
        }

        private async Task ValidateParentObjectAccessAndEditPermissionsAsync(Guid flowID, Guid parentObjectID, CancellationToken cancellationToken = default)
        {
            await ValidateParentEditPermissionsAsync(flowID, cancellationToken);
            await ValidateParentObjectAccessAsync(flowID, parentObjectID, cancellationToken);
        }
        
        private async Task<Negotiation> FindAsync(IFinder<Negotiation> finder, Guid flowID, Guid id, CancellationToken cancellationToken)
        {
            var negotiation = await finder.FindOrRaiseErrorAsync(id, cancellationToken);
            Logger.LogTrace($"FlowID: {flowID}. Negotiation (ID = {id}) is found.");

            return negotiation;
        }

        #endregion

        #region Delete User

        public async Task DeleteNegotiationUserAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var flowID = Guid.NewGuid();
            Logger.LogTrace($"FlowID: {flowID}. User (ID = {CurrentUser.UserId}) is deleting negotiation user (ID = {userId}) from negotiation (ID = {id})");

            var negotiation = await FindAsync(_finder.WithMany(x => x.NegotiationUsers), flowID, id, cancellationToken);
            await ValidateParentObjectAccessAndEditPermissionsAsync(flowID, negotiation.ObjectID, cancellationToken);

            var parentObject = await FindParentObjectAsync(flowID, negotiation.ObjectID, cancellationToken);
            var userToDelete = FindNegotiationUserOrRaiseError(negotiation, userId);

            negotiation.NegotiationUsers.Remove(userToDelete);
            parentObject.UtcDateModified = DateTime.UtcNow;

            if (negotiation.Status == NegotiationStatus.Voting)
            {
                UpdateNegotiationStatus(negotiation);
            }

            await _unitOfWork.SaveAsync(cancellationToken);
            Logger.LogInformation($"FlowID: {flowID}. User (ID = {userId}) is removed from negotiation (ID = {id}) by user (ID = {CurrentUser.UserId}).");

        }

        #endregion

        #region Vote/Comment/Substitute Async

        public async Task<NegotiationUserDetails> UpdateNegotiationUserAsync(Guid id, Guid userID, VoteData data, CancellationToken cancellationToken = default)
        {
            var flowID = Guid.NewGuid();
            Logger.LogTrace($"FlowID: {flowID}. User (ID = {CurrentUser.UserId}) is voting (NegotiationID = {id}) on behalf of user (ID = {userID}).");

            var negotiation = await FindAsync(_finder.WithMany(x => x.NegotiationUsers), flowID, id, cancellationToken);
            var userIsDeputy = _userIsDeputySpecificationBuilder.Build(CurrentUser.UserId);

            if (CurrentUser.UserId != userID && !userIsDeputy.IsSatisfiedBy(userID))
            {
                throw new AccessDeniedException($"Vote / Comment on negotiation (ID = {id}) on behalf of user (ID = {userID}).");
            }
            Logger.LogTrace($"FlowID: {flowID}. Permissions to vote or comment on granted.");

            var negotiationObject = await FindParentObjectAsync(flowID, negotiation.ObjectID, cancellationToken);
            var user = FindNegotiationUserOrRaiseError(negotiation, userID);

            if (negotiation.IsNotStarted) // нельзя голосовать если голосование еще не стартовало
            {
                throw new ObjectReadonlyException(
                    new InframanagerObject(id, ObjectClass.Negotiation));
            }

            if (data.Vote.HasValue)
            {
                if (negotiation.IsFinished)
                {
                    throw new ObjectReadonlyException(
                        new InframanagerObject(negotiation.IMObjID, ObjectClass.Negotiation));
                }

                user.Vote(data.Vote.Value);
                Logger.LogTrace($"FlowID: {flowID}. Voting.");
            }

            if (!string.IsNullOrWhiteSpace(data.Comment))
            {
                user.Comment(data.Comment);
                Logger.LogTrace($"FlowID: {flowID}. Submitting comment.");
            }

            if (data.UserID.HasValue)
            {
                if (user.VotingType != VotingType.None || negotiation.IsFinished)
                {
                    throw new ObjectReadonlyException(new InframanagerObject(id, ObjectClass.Negotiation));
                }

                var oldUser = await _usersRepository.FirstOrDefaultAsync(u => u.IMObjID == user.UserID, cancellationToken);
                var newUser = await _usersRepository.FirstOrDefaultAsync(u => u.IMObjID == data.UserID, cancellationToken)
                    ?? throw new InvalidObjectException("Delegate user is not found.");

                negotiation.NegotiationUsers.Remove(user);
                var existingUser = negotiation.NegotiationUsers.FirstOrDefault(u => u.UserID == data.UserID.Value);

                if (existingUser != null)
                {
                    existingUser.OldUserName = oldUser?.FullName;
                }
                else
                {
                    var newNegotiationUser = user.Copy();
                    newNegotiationUser.UserID = data.UserID.Value;
                    newNegotiationUser.OldUserName = oldUser?.FullName;
                    negotiation.NegotiationUsers.Add(newNegotiationUser);
                }

                Logger.LogTrace($"FlowID: {flowID}. Vote delegating to user (ID = {data.UserID}).");
            }

            UpdateNegotiationStatus(negotiation);
            negotiationObject.UtcDateModified = DateTime.UtcNow;

            await _unitOfWork.SaveAsync(cancellationToken);
            Logger.LogInformation($"FlowID: {flowID}. User (ID = {CurrentUser.UserId}) patched negotiation (ID = {id}) user (ID = {userID}).");

            return _mapper.Map<NegotiationUserDetails>(user);
        }

        private void UpdateNegotiationStatus(Negotiation negotiation)
        {
            negotiation.Status = _statusCalculators
                .Map(negotiation.Mode)
                .Calculate(negotiation.NegotiationUsers);

            if (negotiation.IsFinished)
            {
                negotiation.UtcDateVoteEnd = DateTime.UtcNow;
                Logger.LogInformation($"Negotiation (ID = {negotiation.IMObjID}) is finished.");
            }
        }

        private static NegotiationUser FindNegotiationUserOrRaiseError(Negotiation negotiation, Guid userID)
        {
            return negotiation.NegotiationUsers.FirstOrDefault(x => x.UserID == userID)
                ?? throw new ObjectNotFoundException($"Negotiation user (ID = {negotiation.IMObjID}, UserID = {userID})");
        }

        #endregion
    }
}
