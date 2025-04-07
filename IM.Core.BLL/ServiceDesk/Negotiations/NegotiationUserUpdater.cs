using InfraManager.BLL.Notification;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    // TODO: Назвать класс адекватно тому что он делает
    // TODO: Некорректно отправлять письма в рамках транзакции изменения данных, недоступность почтовых сервисов не должна блокировать работу системы (уведомления должны отправляться асинхронно, с задержкой и гарантированной доставкой) - ничего этого сейчас не реализовано
    public class NegotiationUserUpdater : IVisitDeletedEntity<NegotiationUser>, ISelfRegisteredService<IVisitDeletedEntity<NegotiationUser>>
    {
        private readonly INotificationSenderBLL _notificationSenderBLL;
        private readonly ILogger _logger;

        public NegotiationUserUpdater(INotificationSenderBLL notificationSenderBLL, ILogger<NegotiationUserUpdater> logger)
        {
            _notificationSenderBLL = notificationSenderBLL;
            _logger = logger;
        }

        public void Visit(IEntityState originalState, NegotiationUser entity)
        {
            _notificationSenderBLL.SendNotificationAsync(SystemSettings.NegotiatorDeleteMessageTemplate, new InframanagerObject(entity.NegotiationID, ObjectClass.Negotiation), CancellationToken.None)
                .Wait();
        }

        public async Task VisitAsync(IEntityState originalState, NegotiationUser entity, CancellationToken cancellationToken)
        {
            try
            {
                await _notificationSenderBLL.SendNotificationAsync(SystemSettings.NegotiatorDeleteMessageTemplate, new InframanagerObject(entity.NegotiationID, ObjectClass.Negotiation), cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error sending notification for user {UserID}", entity.UserID);
            }
        }
    }
}
