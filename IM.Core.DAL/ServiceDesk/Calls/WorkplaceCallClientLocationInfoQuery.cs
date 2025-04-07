using InfraManager.DAL.Location;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal class WorkplaceCallClientLocationInfoQuery : IQueryCallClientLocationInfo
    {
        private readonly IWorkplaceQuery _workplaceQuery;

        public WorkplaceCallClientLocationInfoQuery(
                        IWorkplaceQuery workplaceQuery)
        {
            _workplaceQuery = workplaceQuery;
        }

        public async Task<CallClientLocationInfoItem> GetCallClientLocationInfoAsync(Guid locationId, CancellationToken cancellationToken)
        {
            var workplace = await _workplaceQuery.QueryAsync(locationId, cancellationToken);
            if (workplace == null)
                return null;

            var info = new CallClientLocationInfoItem
            {
                PlaceClassID = (int)ObjectClass.Workplace,
                PlaceID = workplace.ID,
                PlaceName = workplace.Name,
                PlaceIntID = workplace.IntID,
                OrganizationName = workplace.OrganizationName,
                BuildingName = workplace.BuildingName,
                FloorName = workplace.FloorName,
                RoomName = workplace.RoomName,
                WorkplaceName = workplace.Name
            };

            return info;
        }
    }
}
