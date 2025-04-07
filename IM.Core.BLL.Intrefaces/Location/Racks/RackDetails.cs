using System;

namespace InfraManager.BLL.Location.Racks
{
    public class RackDetails
    {
        public Guid ID { get; init; }
        public int IntID { get; init; }
        public string Name { get; init; }
        public Guid RoomID { get; init; }
        public int RoomIntID { get; init; }
        public string RoomName { get; init; }
        public Guid FloorID { get; init; }
        public string FloorName { get; init; }
        public Guid BuildingID { get; init; }
        public string BuildingName { get; init; }
        public Guid OrganizationID { get; init; }
        public string OrganizationName { get; init; }
        public string FullName { get; init; }
    }
}
