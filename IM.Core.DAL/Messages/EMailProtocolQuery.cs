using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Notification;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.Negotiations;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;

namespace InfraManager.DAL.Messages
{
    internal class EMailProtocolQuery : IEMailProtocolQuery, ISelfRegisteredService<IEMailProtocolQuery>
    {
        private readonly CrossPlatformDbContext _db;
        private readonly IFinder<DeputyUser> _deputyUsers;

        public EMailProtocolQuery(CrossPlatformDbContext db, IFinder<DeputyUser> deputyUsers)
        {
            _db = db;
            _deputyUsers = deputyUsers;
        }

        public async Task<NotificationReceiver[]> ExecuteForCallAccomplisherAsync(int scope, Guid notificationID, Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            IQueryable<string> query = Query(notificationID, businessRole, scope == 0 ? null : from c in _db.Set<Call>() where c.IMObjID == objectID && c.AccomplisherID!=null select c.AccomplisherID.Value);
            return await getResult(query, cancellationToken);
        }

        public async Task<NotificationReceiver[]> ExecuteForAdministratorAsync(int scope, Guid notificationID, Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            IQueryable<string> query = Query(notificationID, businessRole, OperationID.SD_General_Administrator);
            return await getResult(query, cancellationToken);
        }

        public async Task<NotificationReceiver[]> ExecuteForCallClientAsync(int scope, Guid notificationID, Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            IQueryable<string> query = Query(notificationID, businessRole, scope == 0 ? null : from c in _db.Set<Call>() where c.IMObjID == objectID select c.ClientID);
            return await getResult(query, cancellationToken);
        }

        public async Task<NotificationReceiver[]> ExecuteForControllerParticipantAsync(int scope, Guid notificationID, Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            IQueryable<string> query = Query(notificationID, businessRole, scope == 0 ? null : from cc in _db.Set<CustomControl>() where cc.ObjectId == objectID select cc.UserId);
            return await getResult(query, cancellationToken);
        }

        public async Task<NotificationReceiver[]> ExecuteForDeputyUserAsync(int scope, Guid notificationID,
            Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            var deputyUser = await _deputyUsers.FindAsync(objectID, cancellationToken);
            IQueryable<string> query = Query(notificationID, businessRole, 
                from d in _db.Set<User>() where d.IMObjID == deputyUser.ChildUserId select d.IMObjID );
            return await getResult(query, cancellationToken);
        }

        public async Task<NotificationReceiver[]> ExecuteForCallExecutorAsync(int scope, Guid notificationID, Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            var queryUser = Query(notificationID, businessRole,
                scope == 0
                    ? null
                    : from c in _db.Set<Call>()
                    where c.IMObjID == objectID && c.ExecutorID != null
                    select c.ExecutorID.Value);
            if (scope == 0)
                return await getResult(queryUser, cancellationToken);
            IQueryable<string> queryGroup = Query(notificationID, businessRole, OperationID.SD_General_Executor,
                from gu in _db.Set<GroupUser>()
                join c in _db.Set<Call>() on gu.GroupID equals c.QueueID
                where c.IMObjID == objectID
                select gu.UserID);
            
            var executorID = await (from c in _db.Set<Call>()
                where c.IMObjID == objectID && c.ExecutorID != null
                select c.ExecutorID).FirstOrDefaultAsync(cancellationToken);

            if (executorID == null)
            {
                return await getResult(queryGroup, cancellationToken, ObjectClass.Group, true);
            }

            return await getResult(queryUser, cancellationToken, ObjectClass.User, true);
        }

        public async Task<NotificationReceiver[]> ExecuteForCallInitiatorAsync(int scope, Guid notificationID, Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            IQueryable<string> query = Query(notificationID, businessRole, scope == 0 ? null : from c in _db.Set<Call>() where c.IMObjID == objectID && c.InitiatorID!=null select c.InitiatorID.Value);
            return await getResult(query, cancellationToken);
        }


        public async Task<NotificationReceiver[]> ExecuteForNegotiationParticipantAsync(int scope, Guid notificationID, Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            IQueryable<string> query = Query(
                notificationID,
                businessRole,
                OperationID.SD_General_VotingUser,
                scope == 0 ? null : from nu in _db.Set<NegotiationUser>() where nu.NegotiationID == objectID select nu.UserID);

            return await getResult(query, cancellationToken);
        }

        public async Task<NotificationReceiver[]> ExecuteForCallOwnerAsync(int scope, Guid notificationID, Guid objectID, int businessRole, ObjectClass objectClass, CancellationToken cancellationToken = default)
        {

            IQueryable<string> query = null;
            if(scope == 0)
                query = Query(
                    notificationID,
                    businessRole,
                    OperationID.SD_General_Owner);
            else
                switch (objectClass)
                {
                    case ObjectClass.Call:
                        query = Query(
                            notificationID,
                            businessRole,
                            OperationID.SD_General_Owner,
                            from c in _db.Set<Call>()
                            where c.IMObjID == objectID && c.OwnerID!=null
                            select c.OwnerID.Value
                            );
                        break;
                    case ObjectClass.ChangeRequest:
                    case ObjectClass.Problem:
                        query = Query(
                            notificationID,
                            businessRole,
                            OperationID.SD_General_Owner,
                            from c in _db.Set<Call>()
                            join cr in _db.Set<CallReference>() on c.IMObjID equals cr.CallID
                            where cr.ObjectID == objectID && c.OwnerID!=null
                            select c.OwnerID.Value
                            );
                        break;
                    case ObjectClass.WorkOrder:
                        query = Query(
                            notificationID,
                            businessRole,
                            OperationID.SD_General_Owner,
                            from c in _db.Set<Call>()
                            join wor in _db.Set<WorkOrderReference>() on c.IMObjID equals wor.ObjectID
                            join wo in _db.Set<WorkOrder>() on wor.ID equals wo.WorkOrderReferenceID
                            where wo.IMObjID == objectID && c.OwnerID!=null
                            select c.OwnerID.Value
                            );
                        break;
                }

            

            return await getResult(query, cancellationToken);
        }

        public async Task<NotificationReceiver[]> ExecuteForProblemOwnerAsync(int scope, Guid notificationID, Guid objectID, int businessRole, ObjectClass objectClass, CancellationToken cancellationToken = default)
        {
            IQueryable<string> query = null;
            if (scope == 0)
                query = Query(
                    notificationID,
                    businessRole,
                    OperationID.SD_General_Owner);
            else
                switch (objectClass)
                {
                    case ObjectClass.Call:
                        query = Query(
                            notificationID,
                            businessRole,
                            OperationID.SD_General_Owner,
                            from p in _db.Set<Problem>()
                            join cr in _db.Set<CallReference>() on p.IMObjID equals cr.ObjectID
                            where cr.CallID == objectID && p.OwnerID != null
                            select p.OwnerID.Value
                            );
                        break;
                    case ObjectClass.Problem:
                        query = Query(
                            notificationID,
                            businessRole,
                            OperationID.SD_General_Owner,
                            from p in _db.Set<Problem>()
                            where p.IMObjID == objectID && p.OwnerID != null
                            select p.OwnerID.Value
                            );
                        break;
                    case ObjectClass.ChangeRequest:
                        query = Query(
                            notificationID,
                            businessRole,
                            OperationID.SD_General_Owner,
                            from p in _db.Set<ChangeRequest>()
                            where p.ReasonObjectID == objectID && p.OwnerID != null
                            select p.OwnerID.Value
                            );
                        break;
                    case ObjectClass.WorkOrder:
                        query = Query(
                            notificationID,
                            businessRole,
                            OperationID.SD_General_Owner,
                            from p in _db.Set<Problem>()
                            join wor in _db.Set<WorkOrderReference>() on p.IMObjID equals wor.ObjectID
                            join wo in _db.Set<WorkOrder>() on wor.ID equals wo.WorkOrderReferenceID
                            where wo.IMObjID == objectID && p.OwnerID != null
                            select p.OwnerID.Value
                            );
                        break;
                }


            return await getResult(query, cancellationToken);
        }

        public async Task<NotificationReceiver[]> ExecuteForReplacedUserAsync(int scope, Guid notificationID, Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            var deputyUser = await _deputyUsers.FindAsync(objectID, cancellationToken);
            IQueryable<string> query = Query(notificationID, businessRole, 
                from d in _db.Set<User>() where d.IMObjID == deputyUser.ParentUserId select d.IMObjID);
            return await getResult(query, cancellationToken);
        }

        public async Task<NotificationReceiver[]> ExecuteForRFCInitiatorAsync(int scope, Guid notificationID, Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            IQueryable<string> query = Query(notificationID, businessRole, scope == 0 ? null : from r in _db.Set<ChangeRequest>() where r.IMObjID == objectID && r.InitiatorID != null select r.InitiatorID.Value);
            return await getResult(query, cancellationToken);
        }

        public async Task<NotificationReceiver[]> ExecuteForRFCOwnerAsync(int scope, Guid notificationID, Guid objectID, int businessRole, ObjectClass objectClass, CancellationToken cancellationToken = default)
        {
            IQueryable<string> query = null;
            if (scope == 0)
                query = Query(
                    notificationID,
                    businessRole,
                    OperationID.SD_General_Owner);
            else
                switch (objectClass)
                {
                    case ObjectClass.WorkOrder:
                    case ObjectClass.Problem:
                    case ObjectClass.Call:
                        query = Query(
                            notificationID,
                            businessRole,
                            OperationID.SD_General_Owner,
                            from r in _db.Set<ChangeRequest>()
                            where r.ReasonObjectID == objectID && r.OwnerID != null
                            select r.OwnerID.Value
                            );
                        break;
                    case ObjectClass.ChangeRequest:
                        query = Query(
                            notificationID,
                            businessRole,
                            OperationID.SD_General_Owner,
                            from r in _db.Set<ChangeRequest>()
                            where r.IMObjID == objectID && r.OwnerID != null
                            select r.OwnerID.Value
                            );
                        break;
                }

            return await getResult(query, cancellationToken);
        }

        public async Task<NotificationReceiver[]> ExecuteForWorkOrderAssignorAsync(int scope, Guid notificationID, Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            IQueryable<string> query = Query(
                notificationID,
                businessRole,
                OperationID.SD_General_Executor,
                scope == 0 ? null : from wo in _db.Set<WorkOrder>() where wo.IMObjID == objectID && wo.AssigneeID!=null select wo.AssigneeID.Value);

            return await getResult(query, cancellationToken);
        }

        public async Task<NotificationReceiver[]> ExecuteForWorkOrderExecutorAsync(int scope, Guid notificationID,
            Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            var queryUser = Query(notificationID, businessRole, OperationID.SD_General_Executor,
                scope == 0
                    ? null
                    : from wo in _db.Set<WorkOrder>()
                    where wo.IMObjID == objectID && wo.ExecutorID != null
                    select wo.ExecutorID.Value);
            if (scope == 0)
                return await getResult(queryUser, cancellationToken);
            IQueryable<string> queryGroup = Query(notificationID, businessRole, OperationID.SD_General_Executor, 
                from gu in _db.Set<GroupUser>() 
                join wo in _db.Set<WorkOrder>() on gu.GroupID equals wo.QueueID
                where wo.IMObjID == objectID 
                select gu.UserID);
            
            var executorID = await (from wo in _db.Set<WorkOrder>()
                where wo.IMObjID == objectID && wo.ExecutorID != null
                select wo.ExecutorID).FirstOrDefaultAsync(cancellationToken);

            if (executorID == null)
            {
                return await getResult(queryGroup, cancellationToken, ObjectClass.Group, true);
            }

            return await getResult(queryUser, cancellationToken, ObjectClass.User, true);
        }

        public async Task<NotificationReceiver[]> ExecuteForProblemExecutorAsync(int scope, Guid notificationID,
            Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            var queryUser = Query(notificationID, businessRole, OperationID.SD_General_Executor,
                scope == 0
                    ? null
                    : from wo in _db.Set<Problem>()
                    where wo.IMObjID == objectID && wo.ExecutorID != null
                    select wo.ExecutorID.Value);
            if (scope == 0)
                return await getResult(queryUser, cancellationToken);
            IQueryable<string> queryGroup = Query(notificationID, businessRole, OperationID.SD_General_Executor, 
                from gu in _db.Set<GroupUser>() 
                join wo in _db.Set<Problem>() on gu.GroupID equals wo.QueueID
                where wo.IMObjID == objectID 
                select gu.UserID);

            var executorID = await (from wo in _db.Set<Problem>()
                where wo.IMObjID == objectID
                select wo.ExecutorID).FirstOrDefaultAsync(cancellationToken);

            if (executorID == null)
            {
                return await getResult(queryGroup, cancellationToken, ObjectClass.Group, true);
            }

            return await getResult(queryUser, cancellationToken, ObjectClass.User, true);
        }

        public async Task<NotificationReceiver[]> ExecuteForProblemInitiatorAsync(int scope, Guid notificationID, Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            var query = Query(
                notificationID,
                businessRole,
                OperationID.SD_General_Executor,
                scope == 0
                    ? null
                    : from wo in _db.Set<Problem>()
                    where wo.IMObjID == objectID && wo.InitiatorID != null
                    select wo.InitiatorID.Value);

            return await getResult(query, cancellationToken);
        }
        
        public async Task<NotificationReceiver[]> ExecuteForWorkOrderInitiatorAsync(int scope, Guid notificationID, Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            IQueryable<string> query = Query(
                notificationID,
                businessRole,
                OperationID.SD_General_Executor,
                scope == 0 ? null : from wo in _db.Set<WorkOrder>() where wo.IMObjID== objectID && wo.InitiatorID!=null select wo.InitiatorID.Value);

            return await getResult(query, cancellationToken);
        }

        private IQueryable<string> Query(Guid notificationID, int businessRole, IQueryable<Guid> userIDquery = null)
        {
            var deputyUser = from u in _db.Set<User>()
                             join nu in _db.Set<NotificationUser>() on u.IMObjID equals nu.UserID
                             join d in _db.Set<DeputyUser>() on u.IMObjID equals d.ParentUserId
                             join du in _db.Set<User>() on d.ChildUserId equals du.IMObjID

                             where nu.NotificationID == notificationID
                                 && (du.Email ?? "") != ""
                                 && (nu.BusinessRole & businessRole) == businessRole
                                 && d.UtcDataDeputyWith <= DateTime.UtcNow
                                 && d.UtcDataDeputyBy >= DateTime.UtcNow
                                 && !User.SystemUserIds.Contains(u.ID)
                                 && !u.Removed
                             select u;
            
            if (userIDquery != null)
                deputyUser = from u in deputyUser
                             join f in userIDquery on u.IMObjID equals f
                             select u;
            
            var user = from u in _db.Set<User>()
                       join nu in _db.Set<NotificationUser>() on u.IMObjID equals nu.UserID

                       where nu.NotificationID == notificationID
                           && (u.Email ?? "") != ""
                           && (nu.BusinessRole & businessRole) == businessRole
                       select u;
            if (userIDquery != null)
                user = from u in user
                       join f in userIDquery on u.IMObjID equals f
                       select u;

            return (from du in deputyUser select du.Email).Union(from u in user select u.Email);
        }

        private IQueryable<string> Query(Guid notificationID, int businessRole, OperationID operationId, IQueryable<Guid> userIDquery = null)
        {
            var query = from ur in _db.Set<UserRole>()
                        join ro in _db.Set<RoleOperation>() on ur.RoleID equals ro.RoleID
                        where ro.OperationID == operationId
                        select ur.UserID;

            if (userIDquery != null)
                query = from u in query
                        join f in userIDquery on u equals f
                        select u;

            return Query(notificationID, businessRole, query);
        }

        private IQueryable<string> QueryInt(Guid notificationID, int businessRole, OperationID operationId, IQueryable<int> userIDquery = null)
        {
            var query = from ur in _db.Set<UserRole>()
                join ro in _db.Set<RoleOperation>() on ur.RoleID equals ro.RoleID
                join users in _db.Set<User>() on ur.UserID equals users.IMObjID
                where ro.OperationID == operationId
                select users.ID;

            if (userIDquery != null)
                query = from u in query
                    join f in userIDquery on u equals f
                    select f;

            var newQuery = from user in _db.Set<User>() where query.Contains(user.ID) select user.IMObjID;

            return Query(notificationID, businessRole, newQuery);
        }
        
        #region MassIncident

        public async Task<NotificationReceiver[]> ExecuteForMassIncidentInitiatorAsync(int scope, Guid notificationID,
            Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            var query = QueryInt(
                notificationID,
                businessRole,
                OperationID.SD_General_Executor,
                scope == 0
                    ? null
                    : from wo in _db.Set<MassIncident>() where wo.IMObjID == objectID select wo.CreatedByUserID);

            return await getResult(query, cancellationToken);
        }

        public async Task<NotificationReceiver[]> ExecuteForMassIncidentExecutorAsync(int scope, Guid notificationID,
            Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            var queryUser = QueryInt(notificationID, businessRole,
                OperationID.SD_General_Executor,
                scope == 0
                    ? null
                    : from c in _db.Set<MassIncident>()
                    where c.IMObjID == objectID && c.ExecutedByUserID != User.NullUserId
                    select c.ExecutedByUserID);
        
            if (scope == 0) return await getResult(queryUser, cancellationToken);
        
        
            var hasExecutor = await (from c in _db.Set<MassIncident>()
                where c.IMObjID == objectID && c.ExecutedByUserID != User.NullUserId
                select c.ExecutedByUserID).AnyAsync(cancellationToken);

            if (!hasExecutor)
            {
                IQueryable<string> queryGroup = Query(notificationID, businessRole, OperationID.SD_General_Executor,
                    from gu in _db.Set<GroupUser>()
                    join c in _db.Set<MassIncident>() on gu.GroupID equals c.ExecutedByGroupID
                    where c.IMObjID == objectID
                    select gu.UserID);
            
                return await getResult(queryGroup, cancellationToken, ObjectClass.Group, true);
            }

            return await getResult(queryUser, cancellationToken, ObjectClass.User, true);
        }
        

        public async Task<NotificationReceiver[]> ExecuteForMassIncidentOwnerAsync(int scope, Guid notificationID,
            Guid objectID, int businessRole, CancellationToken cancellationToken = default)
        {
            IQueryable<string> query;
            if (scope == 0)
            {
                query = Query(
                    notificationID,
                    businessRole,
                    OperationID.MassIncident_BeOwner);
            }
            else
            {
                query = QueryInt(
                    notificationID,
                    businessRole,
                    OperationID.MassIncident_BeOwner,
                    from c in _db.Set<MassIncident>()
                    where c.IMObjID == objectID 
                    select c.OwnedByUserID
                );
            }
            
            return await getResult(query, cancellationToken);
        }

        #endregion


        private static async Task<NotificationReceiver[]> getResult(IQueryable<string> query,
            CancellationToken cancellationToken, ObjectClass? objectClass = null, bool? access = null)
        {
            return await (from u in query select new NotificationReceiver(u, objectClass, access)).ToArrayAsync(
                cancellationToken);
        }
    }
}
