using InfraManager.BLL.Notification;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    public class NegotiationUpdater : 
    IVisitModifiedEntity<Negotiation>, 
    ISelfRegisteredService<IVisitModifiedEntity<CallNegotiation>>, ISelfRegisteredService<IVisitModifiedEntity<ProblemNegotiation>>,
    ISelfRegisteredService<IVisitModifiedEntity<ChangeRequestNegotiation>>, ISelfRegisteredService<IVisitModifiedEntity<MassiveIncidentNegotiation>>,
    ISelfRegisteredService<IVisitModifiedEntity<WorkOrderNegotiation>>
    {
        private readonly INotificationSenderBLL _notificationSenderBLL;

        public NegotiationUpdater(INotificationSenderBLL notificationSenderBLL)
        {
            _notificationSenderBLL = notificationSenderBLL;
        }

        public void Visit(IEntityState originalState, Negotiation currentState)
        {
            if (originalState[nameof(Negotiation.Status)].Equals(NegotiationStatus.Created) && currentState.Status == NegotiationStatus.Voting)
            {
                _notificationSenderBLL.SendNotificationAsync(SystemSettings.NegotiationStartMessageTemplate, new InframanagerObject(currentState.IMObjID, ObjectClass.Negotiation), CancellationToken.None)
                    .Wait();
            }
        }

        public async Task VisitAsync(IEntityState originalState, Negotiation currentState, CancellationToken cancellationToken)
        {
            if (originalState[nameof(Negotiation.Status)].Equals(NegotiationStatus.Created) && currentState.Status == NegotiationStatus.Voting)
            {
                await _notificationSenderBLL.SendNotificationAsync(SystemSettings.NegotiationStartMessageTemplate, new InframanagerObject(currentState.IMObjID, ObjectClass.Negotiation), cancellationToken);
            }
        }
    }
}
