using System;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    public class ServiceItemAttendanceItem
    {
        public Guid ID { get; init; }
        
        public string Name { get; init; }
        
        public string Note { get; init; }

        public string Parameter { get; init; }

        public string Summary { get; init; }

        public Guid? ServiceID { get; init; }

        public ObjectClass ObjectClass { get; init; }

        public bool IsInFavorite { get; init; }
    }
}
