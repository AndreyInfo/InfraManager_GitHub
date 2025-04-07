using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Location.TimeZones;

namespace IM.Core.HttpClient.Calendar;

public class CalendarTimeZoneClient : ClientWithAuthorization
{
    private const string _url = "timeZones/";


    public CalendarTimeZoneClient(string baseUrl) : base(baseUrl)
    {
    }
    
    public async Task<TimeZoneDetails> GetAsync(string timeZoneId, Guid? userId = null, CancellationToken cancellationToken = default) 
        => await GetAsync<TimeZoneDetails>($"{_url}{Uri.EscapeDataString(timeZoneId)}", userId, cancellationToken);

    public async Task<TimeZoneDetails[]> GetListAsync(Guid? userID = null, CancellationToken cancellationToken = default)
    {
        return await GetAsync<TimeZoneDetails[]>(_url, userID, cancellationToken);
    }
}