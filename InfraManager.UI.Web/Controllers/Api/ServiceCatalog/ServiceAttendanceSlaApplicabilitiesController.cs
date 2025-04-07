using InfraManager.BLL.ServiceCatalogue;
using InfraManager.UI.Web.Models.ServiceCatalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceCatalog
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceAttendanceSlaApplicabilitiesController : ControllerBase
    {
        private readonly ISlaApplicabilityBLL _slaApplicabilityBLL;

        public ServiceAttendanceSlaApplicabilitiesController(
                    ISlaApplicabilityBLL slaApplicabilityBLL)
        {
            _slaApplicabilityBLL = slaApplicabilityBLL;
        }

        [HttpGet]
        public async Task<SlaApplicabilitiesModel> GetAsync(Guid? userID, int page, int pageSize, CancellationToken cancellationToken)
        {
            var skip = page * pageSize;
            return new SlaApplicabilitiesModel
            {
                ItemIDs = await _slaApplicabilityBLL.AttendanceItemsAsync(userID, ObjectClass.ServiceAttendance, skip, pageSize, cancellationToken)
            };
        }
    }
}
