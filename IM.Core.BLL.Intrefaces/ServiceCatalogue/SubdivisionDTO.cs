using InfraManager.BLL.Users;
using System;

namespace InfraManager.BLL.ServiceCatalogue
{
    public class SubdivisionDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Guid OrganizationID { get; set; }
        public Guid? SubdivisionID { get; set; }
        public string Note { get; set; }
        public string ExternalId { get; set; }
        public Guid? PeripheralDatabaseId { get; set; }
        public Guid? ComplementaryId { get; set; }
        public Guid? CalendarWorkScheduleId { get; set; }
        public bool? IsLockedForOsi { get; set; }
        public bool It { get; set; }

        public UserDTO[] Users { get; set; }
        public SubdivisionDTO[] Subdivisions { get; set; }
    }
}
