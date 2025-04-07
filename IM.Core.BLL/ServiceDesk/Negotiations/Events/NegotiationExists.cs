using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.Negotiations.Events
{
    internal class NegotiationExists : IVerifyEntityStateCondition<NegotiationUser>
    {
        private readonly IFinder<Negotiation> _finder;

        public NegotiationExists(IFinder<Negotiation> finder)
        {
            _finder = finder;
        }

        public bool Matches(IEntityState originalState, NegotiationUser entity)
        {
            return _finder.Find(entity.NegotiationID) != null;
        }
    }
}
