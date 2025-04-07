using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk.Manhours;

namespace InfraManager.BLL.ServiceDesk.Manhours.Events
{
    internal class WorkEventWriterConfigurer : IConfigureEventWriter<ManhoursWork, ManhoursWork>
    {
        public void Configure(IEventWriter<ManhoursWork, ManhoursWork> writer)
        {
            writer.HasParentObject(w => new InframanagerObject(w.ObjectID, w.ObjectClassID));
        }
    }
}
