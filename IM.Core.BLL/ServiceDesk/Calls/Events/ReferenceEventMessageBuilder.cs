using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls.Events
{
    internal class ReferenceEventMessageBuilder<TReference, TObject> : IBuildEventMessage<TReference, TReference>
        where TReference : CallReference
        where TObject : IServiceDeskEntity
    {
        private readonly string _action;
        private readonly IFinder<TObject> _finder;

        public ReferenceEventMessageBuilder(string action, IFinder<TObject> finder)
        {
            _action = action;
            _finder = finder;
        }

        public string Build(TReference entity, TReference subject)
        {
            var referencedObject =  _finder.Find(entity.ObjectID);

            return $"{_action} №{referencedObject.Number}.";
        }
    }
}
