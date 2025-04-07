using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.BLL.Calendar.CalendarWorkSchedules;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Items;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;

namespace InfraManager.UI.Web.Controllers.Api.Calendar.WorkSchedule;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CalendarWorkScheduleController : ControllerBase
{
    private readonly ICalendarWorkScheduleBLL _calendarWorkScheduleBLL;
    public CalendarWorkScheduleController(ICalendarWorkScheduleBLL calendarWorkScheduleBLL)
    {
        _calendarWorkScheduleBLL = calendarWorkScheduleBLL;
    }


    [HttpGet]
    public async Task<CalendarWorkScheduleDetails[]> GetAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken) =>
        await _calendarWorkScheduleBLL.GetAsync(filter, cancellationToken);
    

    [HttpGet("{id}")]
    public async Task<CalendarWorkScheduleWithRelatedDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken) =>
        await _calendarWorkScheduleBLL.GetByIDAsync(id, cancellationToken);


    [HttpPost]
    public async Task<Guid> CreateAsync(CalendarWorkScheduleData calendarWorkScheduleDetails, CancellationToken cancellationToken) =>
        await _calendarWorkScheduleBLL.CreateAsync(calendarWorkScheduleDetails, cancellationToken); 
    
    [HttpPost("analogies")]
    public async Task<Guid> CreateByAnalogiesAsync(CalendarWorkScheduleData calendarWorkScheduleDetails, CancellationToken cancellationToken) =>
        await _calendarWorkScheduleBLL.CreateByAnalogyAsync(calendarWorkScheduleDetails, cancellationToken);


    [HttpPut("{id}")]
    public async Task<Guid> UpdateAsync([FromRoute] Guid id, [FromBody] CalendarWorkScheduleData data, CancellationToken cancellationToken) =>
        await _calendarWorkScheduleBLL.UpdateAsync(id, data, cancellationToken);


    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken) =>
        await _calendarWorkScheduleBLL.RemoveAsync(id, cancellationToken);
    


    [HttpPost("{calendarWorkScheduleID}/Days")]
    public async Task<CalendarWorkScheduleItemDetails[]> GetDaysByIDAsync([FromBody] BaseFilter filter,
                                                                             [FromQuery] DataCalculateWorkSheduleDays model,
                                                                             [FromRoute] Guid calendarWorkScheduleID, CancellationToken cancellationToken) =>
        await _calendarWorkScheduleBLL.GetDaysByIDAsync(filter, calendarWorkScheduleID, model, cancellationToken);

    /// <summary>
    /// Получить исключения всех дней в заданном графике работы.
    /// </summary>
    /// <param name="workScheduleID">Уникальный идентификатор графика работы.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Массив <see cref="CalendarWorkScheduleItemExclusionDetails"/>.</returns>
    [HttpGet("{workScheduleID:guid}/Days/Exclusions")]
    public async Task<CalendarWorkScheduleItemExclusionDetails[]> GetAllExclusionsAsync(
        [FromRoute] Guid workScheduleID,
        CancellationToken cancellationToken = default)
    {
        return await _calendarWorkScheduleBLL.GetExclusionsAsync(workScheduleID, null, cancellationToken);
    }

    /// <summary>
    /// Получить исключения заданного дня в заданном графике работы.
    /// </summary>
    /// <param name="workScheduleID">Уникальный идентификатор графика работы.</param>
    /// <param name="dayOfYear">Номер дня в году.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Массив <see cref="CalendarWorkScheduleItemExclusionDetails"/>.</returns>
    [HttpGet("{workScheduleID:guid}/Days/{dayOfYear:int}/Exclusions")]
    public async Task<CalendarWorkScheduleItemExclusionDetails[]> GetExclusionsAsync(
        [FromRoute] Guid workScheduleID,
        [FromRoute] int dayOfYear,
        CancellationToken cancellationToken = default)
    {
        return await _calendarWorkScheduleBLL.GetExclusionsAsync(workScheduleID, dayOfYear, cancellationToken);
    }
}
