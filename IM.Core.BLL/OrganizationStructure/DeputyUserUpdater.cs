using System;
using InfraManager.BLL.Notification;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.OrganizationStructure;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.OrganizationStructure
{
    public class DeputyUserUpdater :
        IVisitDeletedEntity<DeputyUser>,
        IVisitNewEntity<DeputyUser>,
        IVisitModifiedEntity<DeputyUser>,
        ISelfRegisteredService<IVisitDeletedEntity<DeputyUser>>,
        ISelfRegisteredService<IVisitNewEntity<DeputyUser>>,
        ISelfRegisteredService<IVisitModifiedEntity<DeputyUser>>
    {
        private readonly INotificationSenderBLL _notificationSenderBLL;

        public DeputyUserUpdater(INotificationSenderBLL notificationSenderBLL)
        {
            _notificationSenderBLL = notificationSenderBLL;
        }

        void IVisitDeletedEntity<DeputyUser>.Visit(IEntityState originalState, DeputyUser entity)
        {
            _notificationSenderBLL.SendSeparateNotificationsAsync(
                    SystemSettings.DeleteSubstitution, new InframanagerObject(entity.IMObjID, ObjectClass.Substitution), CancellationToken.None)
                .Wait();
        }

        async Task IVisitDeletedEntity<DeputyUser>.VisitAsync(IEntityState originalState, DeputyUser entity, CancellationToken cancellationToken)
        {
            await _notificationSenderBLL.SendSeparateNotificationsAsync(
                SystemSettings.DeleteSubstitution, new InframanagerObject(entity.IMObjID, ObjectClass.Substitution), cancellationToken);

        }

        void IVisitNewEntity<DeputyUser>.Visit(DeputyUser entity)
        {
            _notificationSenderBLL.SendSeparateNotificationsAsync(
                    SystemSettings.AddSubstitution, new InframanagerObject(entity.IMObjID, ObjectClass.Substitution), CancellationToken.None)
                .Wait();
        }

        async Task IVisitNewEntity<DeputyUser>.VisitAsync(DeputyUser entity, CancellationToken cancellationToken)
        {
            await _notificationSenderBLL.SendSeparateNotificationsAsync(
                SystemSettings.AddSubstitution, new InframanagerObject(entity.IMObjID, ObjectClass.Substitution), cancellationToken);
        }

        void IVisitModifiedEntity<DeputyUser>.Visit(IEntityState originalState, DeputyUser currentState)
        {
            if (currentState.Removed)
            {
                _notificationSenderBLL.SendSeparateNotificationsAsync(SystemSettings.DeleteSubstitution,
                    new InframanagerObject(currentState.IMObjID, ObjectClass.Substitution), CancellationToken.None).Wait();
                return;
            }
            
            if (IsDeputyChanged(originalState, currentState))
                _notificationSenderBLL.SendSeparateNotificationsAsync(SystemSettings.AddSubstitution,
                    new InframanagerObject(currentState.IMObjID, ObjectClass.Substitution), CancellationToken.None)
                .Wait();
            
            if (IsDateChanged(originalState, currentState))
                _notificationSenderBLL.SendSeparateNotificationsAsync(SystemSettings.ChangeDatesSubstitution,
                    new InframanagerObject(currentState.IMObjID, ObjectClass.Substitution), CancellationToken.None)
                .Wait();
        }

        async Task IVisitModifiedEntity<DeputyUser>.VisitAsync(IEntityState originalState, DeputyUser currentState,
            CancellationToken cancellationToken)
        {
            if (currentState.Removed)
            {
                await _notificationSenderBLL.SendSeparateNotificationsAsync(SystemSettings.DeleteSubstitution,
                    new InframanagerObject(currentState.IMObjID, ObjectClass.Substitution), cancellationToken);
                return;
            }
            
            if (IsDeputyChanged(originalState, currentState))
                await _notificationSenderBLL.SendSeparateNotificationsAsync(SystemSettings.AddSubstitution,
                    new InframanagerObject(currentState.IMObjID, ObjectClass.Substitution), cancellationToken);
            
            if (IsDateChanged(originalState, currentState))
                await _notificationSenderBLL.SendSeparateNotificationsAsync(SystemSettings.ChangeDatesSubstitution,
                    new InframanagerObject(currentState.IMObjID, ObjectClass.Substitution), cancellationToken);
        }

        private static bool IsDeputyChanged(IEntityState originalState, DeputyUser currentState)
        {
            return (Guid)originalState["ChildUserId"] != currentState.ChildUserId;
        }

        private static bool IsDateChanged(IEntityState originalState, DeputyUser currentState)
        {
            return (DateTime)originalState["UtcDataDeputyBy"] != currentState.UtcDataDeputyBy || 
                   (DateTime)originalState["UtcDataDeputyWith"] != currentState.UtcDataDeputyWith;
        }
    }
}