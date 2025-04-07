using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk
{
    internal class CallReferenceEventMessageBuilder<TReference> : IBuildEventMessage<CallReference<TReference>, TReference>
        where TReference : IGloballyIdentifiedEntity, IHaveUtcModifiedDate
    {
        private readonly IFinder<Call> _finder;
        private readonly string _action;

        public CallReferenceEventMessageBuilder(IFinder<Call> finder, string action)
        {
            _finder = finder;
            _action = action;
        }

        public string Build(CallReference<TReference> entity, TReference subject)
        {
            var call = _finder.Find(entity.CallID);

            return $"{_action} связь с заявкой №{call.Number}";
        }
    }
}
