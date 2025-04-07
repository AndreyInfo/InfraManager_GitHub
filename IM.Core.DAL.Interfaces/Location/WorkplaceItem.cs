using System;

namespace InfraManager.DAL.Location
{
    public class WorkplaceItem : LocationObjectItem
    {
        public int? RoomID { get; init; }
        
        public string RoomName { get; init; }
    }
}
