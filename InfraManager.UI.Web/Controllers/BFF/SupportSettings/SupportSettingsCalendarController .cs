using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.Settings.Calendar;

namespace InfraManager.UI.Web.Controllers.BFF.SupportSettings;

[Route("api/support-options/calendar")]
[ApiController]
[Authorize]
public class SupportSettingsCalendarController : ControllerBase
{
    private readonly ISupportSettingsCalendarBLL _supportSettingsCalendar;

    public SupportSettingsCalendarController(ISupportSettingsCalendarBLL supportSettingsCalendar)
    {
        _supportSettingsCalendar = supportSettingsCalendar;
    }

    [HttpGet]
    public async Task<CalendarSettingsDetails> GetAsync(CancellationToken cancellationToken) =>
        await _supportSettingsCalendar.GetAsync(cancellationToken);

    [HttpPut]
    public async Task<CalendarSettingsDetails> PutAsync([FromBody] CalendarSettingsDetails settings, CancellationToken cancellationToken) =>
        await _supportSettingsCalendar.UpdateAsync(settings, cancellationToken);

    [HttpPut("{timeZoneID}")]
    public async Task UpdateObjectTimeZoneAsync([FromRoute] string timeZoneID , CancellationToken cancellationToken) =>
        await _supportSettingsCalendar.UpdateObjectTimeZoneAsync(timeZoneID, cancellationToken);
}
