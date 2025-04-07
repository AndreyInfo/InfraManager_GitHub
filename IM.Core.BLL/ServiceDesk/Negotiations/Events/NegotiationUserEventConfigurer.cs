using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.Negotiations.Events
{
    internal class NegotiationUserEventConfigurer : IConfigureEventWriter<NegotiationUser, Negotiation>
    {
        public void Configure(IEventWriter<NegotiationUser, Negotiation> writer)
        {
            writer.HasParentObject((user, negotiation) => new InframanagerObject(negotiation.ObjectID, negotiation.ObjectClassID));
        }
    }
}
