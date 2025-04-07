using AutoMapper;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceCategories;
using InfraManager.UI.Web.Models.ServiceCatalogue;
using InfraManager.Web.BLL.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Settings;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.UI.Web.Controllers.BFF
{
    [Obsolete("Выпилить")]
    [Route("bff/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceCategoriesController : ControllerBase
    {
        private readonly IServiceCategoryBLL _service;
        private readonly IMapper _mapper;
        private readonly IAppSettingsBLL _appSettings;

        public ServiceCategoriesController(IServiceCategoryBLL service,
            IMapper mapper,
            IAppSettingsBLL appSettings)
        {
            _service = service;
            _mapper = mapper;
            _appSettings = appSettings;
        }

        [HttpGet("{category?}/{service?}/{serviceItemAttendance?}")]
        public async Task<ServiceCategoryViewModel[]> GetAllAsync(
            Guid? category,
            Guid? service,
            Guid? serviceItemAttendance,
            [FromQuery] Guid? userID,
            [FromQuery] ServiceType[] types,
            CancellationToken cancellationToken = default)
        {
            var settings = await _appSettings.GetConfigurationAsync(false, cancellationToken);
            var filter = new ServiceCategoryFilter
            {
                CategoryID = category,
                ServiceID = service,
                ServiceItemAttendanceID = serviceItemAttendance,
                UserID = userID,
                AvailableOnly = !settings.WebSettings.VisibleNotAvailableServiceBySla,
                ServiceTypes = types
            };
            var data = await _service.ListAsync(filter, cancellationToken);

            return data.Select(item => _mapper.Map<ServiceCategoryViewModel>(item)).ToArray();
        }
    }
}
