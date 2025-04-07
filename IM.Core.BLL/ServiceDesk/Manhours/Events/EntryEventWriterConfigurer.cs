using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk.Manhours;

namespace InfraManager.BLL.ServiceDesk.Manhours.Events
{
    internal class EntryEventWriterConfigurer : IConfigureEventWriter<ManhoursEntry, ManhoursWork>
    {
        public void Configure(IEventWriter<ManhoursEntry, ManhoursWork> writer)
        {
            writer.HasParentObject((entry, work) => new InframanagerObject(work.ObjectID, work.ObjectClassID));
        }
    }
}
