using InfraManager.Core.Logging;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IM.Core.HttpInfrastructure;

namespace InfraManager.Web.SignalR
{
    public sealed class EventHub : Hub
    {
        #region fields
        //все подключенные клиенты
        private static ConcurrentDictionary<string, Guid> __connectedUsers = new();//signalR.connectionID, userID
        #endregion

        #region static method ProgressBarProcessed
        public static void ProgressBarProcessed(IHubContext<EventHub> context, int objectClassID, Guid objectID, string progressMessage, double percentage, string connectionID)
        {
            Logger.Trace("WorkflowHub.ProgressBarProcessed: objectClassID={0}, objectID={1}, progressMessage={3}, percentage={2}", objectClassID, objectID, progressMessage, percentage);
            GetOthers(context, connectionID).SendAsync("progressBarProcessed", objectClassID, objectID, progressMessage, percentage);//js
        }
        #endregion

        #region static method TS_MessageInserted
        public static void TimeSheet_MessageInserted(IHubContext<EventHub> context, Guid messageID, Guid timesheetID, Guid ownerTimesheetID, Guid authorID, string connectionID)
        {
            Logger.Trace("WorkflowHub.TS_MessageInserted: messageID={0}, timesheetID={1}, ownerTimesheetID={3}, authorID={2}", messageID, timesheetID, ownerTimesheetID, authorID);
            GetOthers(context, connectionID).SendAsync("tsMessageInserted", messageID, timesheetID, ownerTimesheetID, authorID);//js
        }
        #endregion

        #region static method ObjectInserted
        public static void ObjectInserted(IHubContext<EventHub> context, int objectClassID, Guid objectID, Guid? parentObjectID, string connectionID)
        {
            Logger.Trace("WorkflowHub.ObjectInserted: objectClassID={0}, objectID={1}, parentObjectID={2}", objectClassID, objectID, parentObjectID.HasValue ? parentObjectID.Value.ToString() : string.Empty);
            GetOthersWhichCanSeeObject(context, objectClassID, objectID, connectionID).SendAsync("objectInserted", objectClassID, objectID, parentObjectID);//js
        }
        #endregion

        #region static method ObjectUpdated
        public static void ObjectUpdated(IHubContext<EventHub> context, int objectClassID, Guid objectID, Guid? parentObjectID, string connectionID)
        {
            Logger.Trace("WorkflowHub.ObjectUpdated: objectClassID={0}, objectID={1}, parentObjectID={2}", objectClassID, objectID, parentObjectID.HasValue ? parentObjectID.Value.ToString() : string.Empty);
            GetOthersWhichCanSeeObject(context, objectClassID, objectID, connectionID).SendAsync("objectUpdated", objectClassID, objectID, parentObjectID);//js
        }
        #endregion

        #region static method ObjectDeleted
        public static void ObjectDeleted(IHubContext<EventHub> context, int objectClassID, Guid objectID, Guid? parentObjectID, string connectionID)
        {
            Logger.Trace("WorkflowHub.ObjectDeleted: objectClassID={0}, objectID={1}, parentObjectID={2}", objectClassID, objectID, parentObjectID.HasValue ? parentObjectID.Value.ToString() : string.Empty);
            GetOthers(context, connectionID).SendAsync("objectDeleted", objectClassID, objectID, parentObjectID);//js
        }
        #endregion

        #region static method UserSessionChanged
        public static void UserSessionChanged(IHubContext<EventHub> context, Guid userID, string userAgent)
        {
            Logger.Trace("EventHub.UserSessionChanged: userID={0}, userAgent={1}", userID, userAgent);
            context.Clients.All.SendAsync("userSessionChanged", userID, userAgent).Wait();//js
        }
        #endregion

        #region static method WorkflowOnSaveError
        public static void WorkflowOnSaveError(IHubContext<EventHub> context, int objectClassID, Guid objectID, DTL.Workflow.WorkflowResponse response)
        {
            Logger.Trace("WorkflowHub.WorkflowStateChenged: objectClassID={0}, objectID={1}, responseType={2}, responseMessage={3}", objectClassID, objectID, response.Result.ToString(), response.Message);
            context.Clients.All.SendAsync("workflowOnSaveError", objectClassID, objectID, response).Wait();//js
        }
        #endregion

        #region static method ExternalEventCreated
        public static void ExternalEventCreated(IHubContext<EventHub> context, Guid objectID)
        {
            Logger.Trace("WorkflowHub.ExternalEventCreated: objectID={0}", objectID);
            context.Clients.All.SendAsync("externalEventCreated", objectID);//js
        }
        #endregion

        #region static method ExternalEventProcessed
        public static void ExternalEventProcessed(IHubContext<EventHub> context, Guid objectID)
        {
            Logger.Trace("WorkflowHub.ExternalEventProcessed: objectID={0}", objectID);
            context.Clients.All.SendAsync("externalEventProcessed", objectID);
        }
        #endregion


        #region static method CallAnswered
        public static void CallAnswered(IHubContext<EventHub> context, string fromNumber, List<Guid> userIDs)
        {
            Logger.Trace("WorkflowHub.CallAnswered: fromNumber={0}, userIDs={1}", fromNumber, string.Join(", ", userIDs.ToArray()));
            GetByIDs(context, userIDs).SendAsync("callAnswered", fromNumber);//js
        }
        #endregion


        #region override new methods
        
        public async Task ObjectUpdatedAsync(int objectClassID, Guid objectID, Guid? parentObjectID)
        {
           await Clients.AllExcept(Context.ConnectionId).SendAsync("objectUpdated", objectClassID, objectID, parentObjectID);
        }
        
        public async Task ObjectInsertedAsync(int objectClassID, Guid objectID, Guid? parentObjectID)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("objectInserted", objectClassID, objectID, parentObjectID);
        }
        
        public async Task ObjectDeletedAsync(int objectClassID, Guid objectID, Guid? parentObjectID)
        {
            await GetOthersNew(Context.ConnectionId).SendAsync("objectDeleted", objectClassID, objectID, parentObjectID);
        }

        private IClientProxy GetOthersNew(string connectionID)
        {
            return string.IsNullOrEmpty(connectionID) ? Clients.All : Clients.AllExcept(connectionID);
        }
        #endregion

        #region override OnConnected
        public override async Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var userId = new Guid(((ClaimsIdentity)Context.User.Identity).GetUserId());
                __connectedUsers.AddOrUpdate(
                    Context.ConnectionId,
                    userId,
                    (conID, uID) => userId);
            }
            //
            await base.OnConnectedAsync();
        }
        #endregion

        #region overrude OnDisconnected
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Guid userID;
            __connectedUsers.TryRemove(base.Context.ConnectionId, out userID);
            //
            Core.Data.DataSource.CancelAllDBConnectionsByConnectionToken(base.Context.ConnectionId);
            //
            await base.OnDisconnectedAsync(exception);
        }
        #endregion

        #region static method GetOthers
        private static IClientProxy GetOthers(IHubContext<EventHub> context, string connectionID)
        {
            var clients = context.Clients;
            //
            if (!string.IsNullOrWhiteSpace(connectionID))
                return clients.AllExcept(connectionID);
            else
                return clients.All;
        }
        #endregion

        #region static method GetOthersWhichCanSeeObject
        private static IClientProxy GetOthersWhichCanSeeObject(IHubContext<EventHub> context, int objectClassID, Guid objectID, string connectionID)
        {
            var excludeConnectionIDList = new List<string>();
            //
            /*var connections = new Dictionary<string, Guid>(__connectedUsers);
            foreach (KeyValuePair<string, Guid> pair in connections)
                if (pair.Key == connectionID ||
                    !BLL.Helpers.EventHelper.IsUserCanSeeObject(objectID, objectClassID, pair.Value))
                    excludeConnectionIDList.Add(pair.Key);*/
            //            
            var clients = context.Clients;
            return clients.AllExcept(excludeConnectionIDList.ToArray());
        }
        #endregion

        #region static method GetByIDs
        private static dynamic GetByIDs(IHubContext<EventHub> context, List<Guid> userIDs)
        {
            var excludeConnectionIDList = new List<string>();
            //
            var connections = new Dictionary<string, Guid>(__connectedUsers);
            foreach (KeyValuePair<string, Guid> pair in connections)
                if (!userIDs.Contains(pair.Value))
                    excludeConnectionIDList.Add(pair.Key);
            //            
            var clients = context.Clients;
            return clients.AllExcept(excludeConnectionIDList.ToArray());
        }
        #endregion
    }
}