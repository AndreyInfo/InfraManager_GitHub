using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.Negotiations.Events
{
    internal class NegotiationEventConfigurer<TNegotiation> : IConfigureEventWriter<TNegotiation, TNegotiation>
        where TNegotiation : Negotiation
    {
        public void Configure(IEventWriter<TNegotiation, TNegotiation> writer)
        {
            writer.HasParentObject(
                (negotiation, subject) => 
                    new InframanagerObject(negotiation.ObjectID, negotiation.ObjectClassID));
        }
    }
}
