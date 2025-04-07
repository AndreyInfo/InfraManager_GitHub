using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.CalendarWork
{
    /// <summary>
    /// интерфейс для работы с БД для различных сценариев с CalendarExclusion
    /// </summary>
    public interface ICalendarExclusionQuery
    {
        /// <summary>
        /// получение имени сущности, на которую ссылается через ReferenceName
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetNameReferenceServiceAsync(Guid id, CancellationToken cancellationToken);
    }
}
