using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;

namespace InfraManager.BLL.ServiceDesk.Negotiations.Events
{
    internal class NegotiationUserMessageBuilder : IBuildEventMessage<NegotiationUser, Negotiation>
    {
        private readonly IFindEntityByGlobalIdentifier<User> _userFinder;
        private readonly Func<NegotiationUser, Negotiation, User, string> _messagePattern;

        public NegotiationUserMessageBuilder(
            IFindEntityByGlobalIdentifier<User> userFinder,
            Func<NegotiationUser, Negotiation, User, string> messagePattern)
        {
            _userFinder = userFinder;
            _messagePattern = messagePattern;
        }

        public string Build(NegotiationUser entity, Negotiation subject)
        {
            var user = _userFinder.Find(entity.UserID);

            return _messagePattern(entity, subject, user);
        }
    }
}
