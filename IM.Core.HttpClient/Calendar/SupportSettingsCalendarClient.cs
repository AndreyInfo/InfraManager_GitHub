using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Settings.Calendar;

namespace IM.Core.HttpClient.Calendar;

public class SupportSettingsCalendarClient : ClientWithAuthorization
{
    private const string _url = " support-options/calendar/";

    public SupportSettingsCalendarClient(string baseUrl) : base(baseUrl)
    {
    }

    public async Task<CalendarSettingsDetails> GetAsync(Guid? userId = null, CancellationToken cancellationToken = default)
        => await GetAsync<CalendarSettingsDetails>($"{_url}", userId, cancellationToken);

    public async Task<CalendarSettingsDetails[]> GetListAsync(Guid? userID = null, CancellationToken cancellationToken = default)
    {
        var result = await GetAsync(userID, cancellationToken);
        return new CalendarSettingsDetails[] { result };
    }
}