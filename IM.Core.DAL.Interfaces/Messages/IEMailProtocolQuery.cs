using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Messages
{
    public interface IEMailProtocolQuery
    {
        Task<NotificationReceiver[]> ExecuteForAdministratorAsync(int scope, Guid notificationID, Guid objectID, int businessRole,
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForCallOwnerAsync(int scope, Guid notificationID, Guid objectID, int businessRole, ObjectClass objectClass,
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForCallClientAsync(int scope, Guid notificationID, Guid objectID, int businessRole, 
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForCallInitiatorAsync(int scope, Guid notificationID, Guid objectID, int businessRole, 
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForCallAccomplisherAsync(int scope, Guid notificationID, Guid objectID, int businessRole, 
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForCallExecutorAsync(int scope, Guid notificationID, Guid objectID, int businessRole, 
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForProblemOwnerAsync(int scope, Guid notificationID, Guid objectID, int businessRole, ObjectClass objectClass, 
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForWorkOrderExecutorAsync(int scope, Guid notificationID, Guid objectID, int businessRole, 
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForWorkOrderAssignorAsync(int scope, Guid notificationID, Guid objectID, int businessRole, 
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForWorkOrderInitiatorAsync(int scope, Guid notificationID, Guid objectID, int businessRole, 
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForNegotiationParticipantAsync(int scope, Guid notificationID, Guid objectID, int businessRole, 
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForControllerParticipantAsync(int scope, Guid notificationID, Guid objectID, int businessRole, 
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForRFCInitiatorAsync(int scope, Guid notificationID, Guid objectID, int businessRole, 
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForRFCOwnerAsync(int scope, Guid notificationID, Guid objectID, int businessRole, ObjectClass objectClass, 
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForReplacedUserAsync(int scope, Guid notificationID, Guid objectID, int businessRole, 
            CancellationToken cancellationToken = default);
        Task<NotificationReceiver[]> ExecuteForDeputyUserAsync(int scope, Guid notificationID, Guid objectID, int businessRole,
            CancellationToken cancellationToken = default);

        Task<NotificationReceiver[]> ExecuteForProblemExecutorAsync(int scope, Guid notificationID,
            Guid objectID, int businessRole, CancellationToken cancellationToken = default);

        Task<NotificationReceiver[]> ExecuteForProblemInitiatorAsync(int scope, Guid notificationID, Guid objectID,
            int businessRole, CancellationToken cancellationToken = default);

        Task<NotificationReceiver[]> ExecuteForMassIncidentInitiatorAsync(int scope, Guid notificationID,
            Guid objectID, int businessRole, CancellationToken cancellationToken = default);

        Task<NotificationReceiver[]> ExecuteForMassIncidentOwnerAsync(int scope, Guid notificationID,
            Guid objectID, int businessRole, CancellationToken cancellationToken = default);

        Task<NotificationReceiver[]> ExecuteForMassIncidentExecutorAsync(int scope, Guid notificationID,
            Guid objectID, int businessRole, CancellationToken cancellationToken = default);
    }
}
