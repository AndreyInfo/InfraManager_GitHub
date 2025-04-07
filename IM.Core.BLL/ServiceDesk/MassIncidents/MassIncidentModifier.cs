using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.Settings;
using InfraManager.BLL.Workflow;
using InfraManager.Core.Extensions;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentModifier : 
        IModifyObject<MassIncident, MassIncidentData>,
        ISelfRegisteredService<IModifyObject<MassIncident, MassIncidentData>>
    {
        private readonly IMapper _mapper;
        private readonly IUserAccessBLL _userAccess;
        private readonly IFindEntityByGlobalIdentifier<User> _userFinder;
        private readonly IFindEntityByGlobalIdentifier<Group> _groupFinder;
        private readonly ICurrentUser _currentUser;
        private readonly IReadonlyRepository<TechnicalFailureCategory> _techFailureCategories;
        private readonly ISetAgreement<MassIncident> _slaSetter;
        private readonly ISelectWorkflowScheme<MassIncident> _workflowSchemeProvider;
        private readonly IRepository<ManyToMany<MassIncident, Service>> _affectedServices;

        public MassIncidentModifier(
            IMapper mapper,
            IUserAccessBLL userAccess, 
            IFindEntityByGlobalIdentifier<User> userFinder,
            IFindEntityByGlobalIdentifier<Group> groupFinder,
            ICurrentUser currentUser,
            IReadonlyRepository<TechnicalFailureCategory> techFailureCategories,
            ISetAgreement<MassIncident> slaSetter,
            ISelectWorkflowScheme<MassIncident> workflowSchemeProvider,
            IRepository<ManyToMany<MassIncident, Service>> affectedServices)
        {
            _mapper = mapper;
            _userAccess = userAccess;
            _userFinder = userFinder;
            _groupFinder = groupFinder;
            _currentUser = currentUser;
            _techFailureCategories = techFailureCategories;
            _slaSetter = slaSetter;
            _workflowSchemeProvider = workflowSchemeProvider;
            _affectedServices = affectedServices;
        }

        public async Task ModifyAsync(MassIncident entity, MassIncidentData data, CancellationToken cancellationToken = default)
        {
            //TODO: Перевести WE на PATCH запросы и убрать этот костыль
            var groupChanged = !data.GroupID.Ignore
                && ((data.GroupID.IsEmpty && entity.ExecutedByGroupID != Group.NullGroupID)
                    || (!data.GroupID.IsEmpty && entity.ExecutedByGroupID != data.GroupID.Value));
            var executorChanged = !data.ExecutedByUserID.Ignore
                && ((data.ExecutedByUserID.IsEmpty && entity.ExecutedByUserID != User.NullUserId)
                    || (!data.ExecutedByUserID.IsEmpty && entity.ExecutedByUser.IMObjID != data.ExecutedByUserID.Value));
            var massIncidentTypeChanged = data.TypeID.HasValue && entity.TypeID != data.TypeID;

            if (data.ServiceFieldSet
                && !(await _userAccess.UserHasOperationAsync(_currentUser.UserId, OperationID.MassIncident_EditServiceFields, cancellationToken)))
            {
                throw new AccessDeniedException($"{nameof(OperationID.MassIncident_EditServiceFields)} of mass incident (ID = {entity.ID})");
            }

            _mapper.Map(data, entity);

            if (!data.OwnedByUserID.Ignore 
                && entity.OwnedBy.IMObjID != (data.OwnedByUserID.Value ?? User.NullUserGloablIdentifier))
            {
                if (entity.OwnedBy.ID != User.NullUserId 
                    && !await _userAccess.UserHasOperationAsync(
                        _currentUser.UserId, 
                        OperationID.MassIncident_ChangeOwner, 
                        cancellationToken))
                {
                    throw new AccessDeniedException("Change mass incident owner");
                }

                entity.OwnedBy = await GetUserAsync(data.OwnedByUserID, nameof(entity.OwnedBy), cancellationToken);
            }

            if (!data.CreatedByUserID.Ignore)
            {
                entity.CreatedBy = await GetUserAsync(data.CreatedByUserID, nameof(entity.CreatedBy), cancellationToken);
            }

            if (executorChanged)
            {
                entity.ExecutedByUser = await GetUserAsync(data.ExecutedByUserID, nameof(entity.ExecutedByUser), cancellationToken);

                if (!groupChanged //TODO: Выпилить костыль и заниматься этой ерундой на клиенте/WE
                    && MassIncident.ExecutedByUserAndGroup.IsSatisfiedBy(entity)
                    && !MassIncident.UserIsInGroup.Build(entity.ExecutedByUser).IsSatisfiedBy(entity))
                {
                    entity.ExecutedByGroup = await _groupFinder.FindAsync(Group.NullGroupID, cancellationToken);
                }
            }

            if (groupChanged)
            {
                entity.ExecutedByGroup = await GetGroupAsync(data.GroupID, nameof(entity.ExecutedByGroup), cancellationToken);

                if (!executorChanged //TODO: Выпилить костыль и заниматься этой ерундой на клиенте/WE                    
                    && MassIncident.ExecutedByUserAndGroup.IsSatisfiedBy(entity)
                    && !MassIncident.UserIsInGroup.Build(entity.ExecutedByUser).IsSatisfiedBy(entity))
                {
                    entity.ExecutedByUser = await GetNullUserAsync(cancellationToken);
                }
            }

            var techFailureCategory = await _techFailureCategories
                .WithMany(x => x.Services)
                .ThenWith(x => x.Reference)
                .FirstOrDefaultAsync(x => x.ID == entity.TechnicalFailureCategoryID, cancellationToken);

            if (data.ServiceID.HasValue)
            {
                if (entity.TechnicalFailureCategoryID.HasValue
                        && (techFailureCategory == null 
                            || !TechnicalFailureCategory.AvailableForService.Build(entity.ServiceID).IsSatisfiedBy(techFailureCategory)))
                {
                    entity.TechnicalFailureCategoryID = null;
                }

                var matchingAffectedService = entity.AffectedServices.FirstOrDefault(x => x.Reference.ID == entity.ServiceID);
                if (matchingAffectedService != null)
                {
                    _affectedServices.Delete(matchingAffectedService);
                }
            }        

            if (data.RefreshAgreement != null)
            {
                await _slaSetter.SetAsync(entity, cancellationToken, data.RefreshAgreement?.CountUtcCloseDateFrom, data.RefreshAgreement?.AgreementID);
            }

            if (massIncidentTypeChanged)
            {
                entity.WorkflowSchemeIdentifier = await _workflowSchemeProvider.SelectIdentifierAsync(entity, cancellationToken);
            }            
        }

        private async Task<User> GetUserAsync(
            NullablePropertyWrapper<Guid> userID, 
            string role,
            CancellationToken cancellationToken = default)
        {
            return userID.IsEmpty
                ? await GetNullUserAsync(cancellationToken)
                : await _userFinder.FindAsync(userID.Value.Value, cancellationToken) 
                  ?? throw new InvalidObjectException($"{role} not found.");;
        }

        private async Task<Group> GetGroupAsync(
            NullablePropertyWrapper<Guid> groupID,
            string role,
            CancellationToken cancellationToken = default)
        {
            return groupID.IsEmpty
                ? await _groupFinder.FindAsync(Group.NullGroupID, cancellationToken)
                : await _groupFinder.FindAsync(groupID.Value.Value, cancellationToken)
                  ?? throw new InvalidObjectException($"{role} not found."); ;
        }

        private async Task<User> GetNullUserAsync(CancellationToken cancellationToken = default) =>
            await _userFinder.FindAsync(User.NullUserGloablIdentifier, cancellationToken);

        public void SetModifiedDate(MassIncident entity)
        {
            entity.UtcDateModified = DateTime.UtcNow;
        }
    }
}
