using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager;
using InfraManager.BLL.Calendar;
using InfraManager.BLL.Calendar.CalendarExclusions;

namespace IM.Core.HttpClient.Calendar
{
    public class CalendarExclusionsClient : ClientWithAuthorization
    {
        private const string Path = "CalendarExclusions";

        public CalendarExclusionsClient(string baseUrl) : base(baseUrl) { }


        public Task<CalendarExclusionDetails[]> GetListByObjectIDAsync(ObjectClass objectClass, Guid id, CancellationToken cancellationToken = default) =>
            PostAsync<CalendarExclusionDetails[], BaseFilterWithClassIDAndID<Guid>>($"{Path}/byFilter", new BaseFilterWithClassIDAndID<Guid> { ClassID = objectClass, ObjectID = id }, cancellationToken: cancellationToken);
    }
}