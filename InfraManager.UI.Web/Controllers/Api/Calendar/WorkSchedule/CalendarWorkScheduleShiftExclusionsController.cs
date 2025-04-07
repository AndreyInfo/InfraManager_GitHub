using InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace InfraManager.UI.Web.Controllers.Api.Calendar.WorkSchedule;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CalendarWorkScheduleShiftExclusionsController : ControllerBase
{
    private readonly ICalendarWorkScheduleShiftExlusionsBLL _bll;

    public CalendarWorkScheduleShiftExclusionsController(ICalendarWorkScheduleShiftExlusionsBLL bll)
    {
        _bll = bll;
    }

    [HttpGet]
    public async Task<CalendarWorkScheduleShiftExclusionDetails[]> GetExclusionsAsync([FromQuery] FilterShiftsExclusion filter, CancellationToken cancellationToken)
    {
        return await _bll.GetExclutionsAsync(filter, cancellationToken);
    }

    [HttpGet("{id}/totaltime")]
    public async Task<int> GetExclusionsTotalTimeAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _bll.GetExclutionsTotalTimeAsync(id, cancellationToken);
    }


    [HttpPost]
    public async Task<Guid> CreateExclutionAsync([FromBody] CreateCalendarWorkScheduleShiftExclusionDetails dto, CancellationToken cancellationToken)
    {
        return await _bll.CreateExclutionAsync(dto, cancellationToken);
    }

    [HttpPut]
    public async Task<Guid> UpdateExclusionAsync([FromBody] CalendarWorkScheduleShiftExclusionDetails dto, CancellationToken cancellationToken)
    {
        await _bll.UpdateExclutionAsync(dto, cancellationToken);

        return dto.ID;
    }

    [HttpDelete("{id}")]
    public async Task DeleteExclusionAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _bll.DeleteExclusionAsync(id, cancellationToken);
    }
}
