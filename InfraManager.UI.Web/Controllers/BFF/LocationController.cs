using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.Location;
using System;

namespace InfraManager.UI.Web.Controllers.BFF
{
    [Authorize]
    [Route("api/location/")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationBLL _locationBLL;
        public LocationController(ILocationBLL locationBLL)
        {
            _locationBLL = locationBLL;
        }

        [HttpPost("tree")]
        public async Task<LocationTreeNodeDetails[]> GetTreeLocation([FromBody] LocationTreeFilter model)
        {
            return await _locationBLL.GetLocationNodesByParentIdAndRightsUserAsync(model);
        }

        [HttpGet("path")]
        public async Task<LocationTreeNodeDetails[]> GetPath(ObjectClass ClassID, Guid UID)
        {
            return await _locationBLL.GetBranchTreeAsync(ClassID, UID);
        }


        [HttpGet("nodes/organization")]
        public async Task<LocationTreeNodeDetails[]> GetOrganizationNodesAsync(CancellationToken cancellationToken)
        {
            return await _locationBLL.GetOrganizationNodesAsync(cancellationToken);
        }

        [HttpGet("nodes/building")]
        public async Task<LocationTreeNodeDetails[]> GetBuildingNodesAsync([FromQuery] Guid parentID, CancellationToken cancellationToken)
        {
            return await _locationBLL.GetBuildingNodesAsync(parentID, cancellationToken);
        }


        [HttpGet("nodes/floor")]
        public async Task<LocationTreeNodeDetails[]> GetFloorNodesAsync([FromQuery] int parentID, CancellationToken cancellationToken)
        {
            return await _locationBLL.GetFloorNodesAsync(parentID, cancellationToken);
        }

        [HttpGet("nodes/room")]
        public async Task<LocationTreeNodeDetails[]> GetRoomNodesAsync([FromQuery] int parentID, [FromQuery] ObjectClass? childClassID, CancellationToken cancellationToken)
        {
            return await _locationBLL.GetRoomNodesAsync(parentID, childClassID, cancellationToken);
        }


        [HttpGet("nodes/workplace")]
        public async Task<LocationTreeNodeDetails[]> GetWorkplaceNodesAsync([FromQuery] int parentID, CancellationToken cancellationToken)
        {
            return await _locationBLL.GetWorkplaceNodesAsync(parentID, cancellationToken);
        }

        [HttpGet("nodes/rack")]
        public async Task<LocationTreeNodeDetails[]> GetRackNodesAsync([FromQuery] int parentID, CancellationToken cancellationToken)
        {
            return await _locationBLL.GetRackNodesAsync(parentID, cancellationToken);
        }
    }
}
