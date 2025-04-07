using System;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    public class CallClientLocationInfoItem
    {
        public Guid? PlaceID { get; init; }

        public string PlaceName { get; init; }

        public int? PlaceIntID { get; init; }

        public int? PlaceClassID { get; init; }

        public string OrganizationName { get; init; }
        
        public string BuildingName { get; init; }
        
        public string FloorName { get; init; }

        public string RoomName { get; init; }

        public string WorkplaceName { get; init; }
    }
}
