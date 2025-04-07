using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls.Events
{
    internal class CallEventWriterConfigurer : IConfigureEventWriter<Call, Call>
    {
        public void Configure(IEventWriter<Call, Call> writer)
        {
            // Пока Parent-ов и других настроек нет
        }
    }
}
