using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk.Manhours;

namespace InfraManager.BLL.ServiceDesk.Manhours.Events
{
    internal class EntrySubjectFinder : ISubjectFinder<ManhoursEntry, ManhoursWork>
    {
        private readonly IFinder<ManhoursWork> _finder;

        public EntrySubjectFinder(IFinder<ManhoursWork> finder)
        {
            _finder = finder;
        }

        public ManhoursWork Find(ManhoursEntry entity, IEntityState originalState)
        {
            return _finder.Find(entity.WorkID);
        }
    }
}
