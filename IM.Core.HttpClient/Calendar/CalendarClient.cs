using InfraManager.BLL.CalendarService;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Calendar.CalendarHolidays;
using InfraManager.BLL.Calendar.CalendarWeekends;
using InfraManager.BLL.Settings.Calendar;

namespace IM.Core.HttpClient.Calendar
{
    public class CalendarClient : ClientWithAuthorization
    {
        internal static string _url = "workTimeCalculationRequests/";
        public CalendarClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<WorkTimeInfo> GetWorkTimeByCalendarAsync(WorkTimeByCalendarData data, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<WorkTimeInfo, WorkTimeByCalendarData>($"{_url}worktime-by-calendar", data, userId, cancellationToken);
        }
        public async Task<WorkTimeInfo> GetWorkTimeByUserAsync(WorkTimeByUserData data, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<WorkTimeInfo, WorkTimeByUserData>($"{_url}worktime-by-user", data, userId, cancellationToken);
        }
        public async Task<WorkDateInfo> GetWorkDateByCalendarAsync(WorkDateByCalendarData data, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<WorkDateInfo, WorkDateByCalendarData>($"{_url}workdate-by-calendar", data, userId, cancellationToken);
        }
        public async Task<WorkDateInfo> GetWorkDateByUserAsync(WorkDateByUserData data, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<WorkDateInfo, WorkDateByUserData>($"{_url}workdate-by-user", data, userId, cancellationToken);
        }

        public async Task<CalendarSettingsDetails> GetDefaultCalendarSettingsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<CalendarSettingsDetails>("support-options/calendar", cancellationToken: cancellationToken);
        }

        public async Task<CalendarWeekendDetails> GetWeekendsAsync(Guid calendarId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<CalendarWeekendDetails>($"calendarweekends/{calendarId}", cancellationToken: cancellationToken);
        }

        public async Task<CalendarHolidayDetails> GetHolidayAsync(Guid calendarId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<CalendarHolidayDetails>($"calendarholidays/{calendarId}", cancellationToken: cancellationToken);
        }

        public async Task<CalendarHolidayItemDetails> GetHolidayItemAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<CalendarHolidayItemDetails>($"calendarholidayitems/{id}", cancellationToken: cancellationToken);
        }
    }
}