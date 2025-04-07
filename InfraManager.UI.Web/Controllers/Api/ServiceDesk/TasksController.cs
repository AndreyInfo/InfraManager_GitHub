using AutoMapper;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.MyTasks
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly IMyTaskBLL _service;
        private readonly IMapper _mapper;

        public TasksController(IMyTaskBLL service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("reports/mytasks")]
        public async Task<ListItemModel[]> ListAsync([FromQuery]ListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var tasks = await _service.GetListAsync(filterBy.ToServiceDeskFilter(), cancellationToken);

            return tasks.Select(task => _mapper.Map<ListItemModel>(task)).ToArray();
        }
    }
}
