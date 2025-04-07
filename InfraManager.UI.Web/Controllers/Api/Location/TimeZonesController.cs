using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Location.TimeZones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeZoneModel = InfraManager.DAL.ServiceDesk;

namespace InfraManager.UI.Web.Controllers.Api.Location
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TimeZonesController : ControllerBase
    {
        private readonly ITimeZoneBLL _timeZoneBLL;
        public TimeZonesController(ITimeZoneBLL timeZoneBLL) => _timeZoneBLL = timeZoneBLL;

        [HttpGet]
        public async Task<IEnumerable<TimeZoneDetails>> GetListTimeZone([FromQuery] string search,
            CancellationToken cancellationToken = default)
        {
            var filter = new TimeZoneListFilter
            {
                Name = search,
                OrderByProperty = nameof(TimeZoneDetails.BaseUtcOffsetInMinutes),
                Ascending = true
            };
            return await _timeZoneBLL.GetDataForTableAsync(filter, cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<TimeZoneDetails> GetAsync([FromRoute] string id,
            CancellationToken cancellationToken = default)
        => await _timeZoneBLL.GetAsync(WebUtility.UrlDecode(id), cancellationToken);
        
    }
}
