using Inframanager.BLL.Events;
using InfraManager.DAL.Events;
using InfraManager.DAL.ServiceDesk.Manhours;

namespace InfraManager.BLL.ServiceDesk.Manhours.Events
{
    internal class EventSubjectBuilder :
        IBuildEventSubject<ManhoursWork, ManhoursWork>,
        IBuildEventSubject<ManhoursEntry, ManhoursWork>
    {
        public EventSubject Build(ManhoursWork subject)
        {
            return new EventSubject(
                "Работа", 
                $"Работа №{subject.Number}", 
                new InframanagerObject(subject.IMObjID, ObjectClass.ManhoursWork));
        }
    }
}
