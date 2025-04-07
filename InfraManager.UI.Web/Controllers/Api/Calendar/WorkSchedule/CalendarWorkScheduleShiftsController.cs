using InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.WorkSchedule
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CalendarWorkScheduleShiftsController : ControllerBase
    {
        private readonly ICalendarWorkScheduleShiftsBLL _bll;

        public CalendarWorkScheduleShiftsController(ICalendarWorkScheduleShiftsBLL calendarWorkScheduleShiftsBLL)
        {
            _bll = calendarWorkScheduleShiftsBLL;
        }

        [HttpGet("{id}")]
        public async Task<CalendarWorkScheduleShiftDetails> GetByIDAsync([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            return await _bll.GetByIDAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<Guid?> AddAsync(CalendarWorkScheduleShiftCreateData dto, CancellationToken cancellationToken)
        {
            return await _bll.AddAsync(dto, cancellationToken);
        }

        [HttpPut]
        public async Task UpdateAsync(CalendarWorkScheduleShiftDetails calendarWorkScheduleDetails, CancellationToken cancellationToken)
        { 
            await _bll.UpdateAsync(calendarWorkScheduleDetails, cancellationToken);
        }

        /// <summary>
        /// Стоит узнать: стоит ли вызывать автоматически ReCalculateDays
        /// Сейчас этого не происходит
        /// </summary>
        /// <param name="calendarWorkScheduleDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _bll.RemoveAsync(id, cancellationToken);
        }

        [HttpGet("{calendarWorkScheduleID}/Shifts")]
        public async Task<CalendarWorkScheduleShiftDetails[]> GetShiftsByIDAsync([FromRoute] Guid calendarWorkScheduleID, CancellationToken cancellationToken) =>
            await _bll.GetShiftsByIDAsync(calendarWorkScheduleID, cancellationToken);

    }
}
