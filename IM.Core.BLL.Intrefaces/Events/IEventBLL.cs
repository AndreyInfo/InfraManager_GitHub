using InfraManager.DAL.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Events
{
    public interface IEventBLL
    {
        /// <summary>
        /// Добавить event в DbSet
        /// </summary>
        void AddEvent(Event @event);

        /// <summary>
        /// Добавить event в DbSet и применить изменения.
        /// </summary>
        Task AddEventSaveAsync(Event @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает массив событий для объекта
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        Task<Event[]> GetEventsAsync(Guid id, DateTime? dateFrom, DateTime? dateTo, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает список объектов EventSubject
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="cancellationToken"></param>
        /// <returns>список объектов EventSubjects</returns>
        Task<EventDetails[]> GetEventSubjectsAsync(EventSubjectFilter filter, CancellationToken cancellationToken);
        /// <summary>
        /// Возвращает EventSubject
        /// </summary>
        /// <param name="id">Идентификатор события</param>
        /// <param name="cancellationToken"></param>
        /// <returns>объект EventSubject</returns>
        Task<EventDetails> GetEventSubjectAsync(Guid id, CancellationToken cancellationToken);
    }
}
