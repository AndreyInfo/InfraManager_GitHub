using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls.Events
{
    internal class ReferenceEventWriterConfigurer<T> : IConfigureEventWriter<T, T>
        where T : CallReference
    {
        public void Configure(IEventWriter<T, T> writer)
        {
            writer.HasParentObject(
                callRef => new InframanagerObject(callRef.CallID, ObjectClass.Call));
        }
    }
}
