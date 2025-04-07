using AutoMapper;
using InfraManager.BLL;
using InfraManager.BLL.CalendarService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Calendar
{
    [Route("api/[controller]")]
    [ApiController]
    public class workTimeCalculationRequestsController : ControllerBase
    {
        private readonly ICalendarServiceBLL _service;
        private readonly IMapper _mapper;

        public workTimeCalculationRequestsController(ICalendarServiceBLL serviceBLL, IMapper mapper)
        {
            _service = serviceBLL;
            _mapper = mapper;
        }

        [HttpPost("worktime-by-calendar")]
        public async Task<WorkTimeInfo> GetWorkTimeByCalendar(WorkTimeByCalendarData data, CancellationToken cancellationToken = default)
        {
            if(data == null)
            {
                return new WorkTimeInfo() { TimeSpan = null };
            }

            TimeSpan timeSpan = await _service.GetWorkTimeByCalendarAsync(data, cancellationToken);

            return new WorkTimeInfo() { TimeSpan = timeSpan };
        }

        [HttpPost("worktime-by-user")]
        public async Task<WorkTimeInfo> GetWorkTimeByUser(WorkTimeByUserData data, CancellationToken cancellationToken = default)
        {
            if(data == null)
            {
                return new WorkTimeInfo() { TimeSpan = null };
            }

            TimeSpan timeSpan = await _service.GetWorkTimeByUserAsync(data, cancellationToken);

            return new WorkTimeInfo() { TimeSpan = timeSpan };
        }

        [HttpPost("workdate-by-calendar")]
        public async Task<WorkDateInfo> GetWorkDateByCalendar(WorkDateByCalendarData data, CancellationToken cancellationToken = default)
        {
            if (data == null)
            {
                return new WorkDateInfo() { DateTime = null };
            }

            var result = await _service.GetWorkDateByCalendarAsync(data, cancellationToken);

            return new WorkDateInfo() { DateTime = result };
        }

        [HttpPost("workdate-by-user")]
        public async Task<WorkDateInfo> GetWorkDateByUser(WorkDateByUserData data, CancellationToken cancellationToken = default)
        {
            if (data == null)
            {
                return new WorkDateInfo() { DateTime = null };
            }

            var result = await _service.GetWorkDateByUserAsync(data, cancellationToken);

            return new WorkDateInfo() { DateTime = result };
        }

    }
}
