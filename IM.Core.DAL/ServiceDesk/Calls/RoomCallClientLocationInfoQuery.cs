using System.Threading.Tasks;
using System.Threading;
using System;
using InfraManager.DAL.Location;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal class RoomCallClientLocationInfoQuery : IQueryCallClientLocationInfo
    {
        private readonly IRoomQuery _roomQuery;

        public RoomCallClientLocationInfoQuery(IRoomQuery roomQuery)
        {
            _roomQuery = roomQuery;
        }

        public async Task<CallClientLocationInfoItem> GetCallClientLocationInfoAsync(Guid locationId, CancellationToken cancellationToken)
        {
            var room = await _roomQuery.QueryAsync(locationId, cancellationToken);
            if (room == null)
                return null;

            var info = new CallClientLocationInfoItem
            {
                PlaceClassID = (int)ObjectClass.Room,
                PlaceID = room.ID,
                PlaceName = room.Name,
                PlaceIntID = room.IntID,
                OrganizationName = room.OrganizationName,
                BuildingName = room.BuildingName,
                FloorName = room.FloorName,
                RoomName = room.Name
            };

            return info;
        }
    }
}
