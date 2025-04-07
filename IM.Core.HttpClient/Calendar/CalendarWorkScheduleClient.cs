using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Calendar.CalendarWorkSchedules;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Items;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;

namespace IM.Core.HttpClient.Calendar
{
    public class CalendarWorkScheduleClient : ClientWithAuthorization
    {
        internal static string _url = "calendarWorkSchedule/";
        public CalendarWorkScheduleClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<CalendarWorkScheduleDetails[]> GetListAsync(Guid? userID, CancellationToken cancellationToken = default)
        {
            return await GetListAsync<CalendarWorkScheduleDetails[]>(_url, userID, cancellationToken);
        }

        public async Task<CalendarWorkScheduleWithRelatedDetails> GetExclusionListAsync(Guid ID, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<CalendarWorkScheduleWithRelatedDetails>($"{_url}{ID}", userId, cancellationToken);
        }

        public async Task<CalendarWorkScheduleItemExclusionDetails[]> GetExclusionListAsync(Guid workScheduleID, int? dayOfYear, Guid? userID = null, CancellationToken cancellationToken = default)
        {
            var url = dayOfYear.HasValue
                ? $"{_url}{workScheduleID}/Days/{dayOfYear.Value}/Exclusions"
                : $"{_url}{workScheduleID}/Days/Exclusions";
            return await GetAsync<CalendarWorkScheduleItemExclusionDetails[]>(url, userID, cancellationToken);
        }

        public async Task<CalendarWorkScheduleItemDetails[]> WorkScheduleItemAsync(Guid workScheduleID, Guid? userID = null, CancellationToken cancellationToken = default)
        {
            var url = $"{_url}{workScheduleID}/Days";
            var request = new { ViewName = "DaysForTable", };
            return await PostAsync<CalendarWorkScheduleItemDetails[], object>(url, request, userID, cancellationToken);
        }
    }
}
