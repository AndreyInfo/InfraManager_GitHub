using System;

namespace InfraManager.DAL.Location
{
    public abstract class LocationObjectItem
    {
        public Guid ID { get; init; }

        public int IntID { get; init; }

        public string Name { get; init; }

        public Guid? OrganizationID { get; init; }

        public string OrganizationName { get; init; }

        public int? BuildingID { get; init; }

        public string BuildingName { get; init; }

        public int? FloorID { get; init; }

        public string FloorName { get; init; }
    }
}
