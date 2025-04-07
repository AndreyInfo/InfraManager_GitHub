using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.Negotiations.Events
{
    internal class NegotiationUserSubjectFinder : ISubjectFinder<NegotiationUser, Negotiation>
    {
        private readonly IFinder<Negotiation> _finder;

        public NegotiationUserSubjectFinder(IFinder<Negotiation> finder)
        {
            _finder = finder;
        }

        public Negotiation Find(NegotiationUser entity, IEntityState originalState)
        {
            return _finder.Find(entity.NegotiationID);
        }
    }
}
