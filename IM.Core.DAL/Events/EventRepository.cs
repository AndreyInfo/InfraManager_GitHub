using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace InfraManager.DAL.Events
{
    internal class EventRepository : Repository<Event>,
        IEventRepository,
        ISelfRegisteredService<IEventRepository>
    {
        private readonly ConcurrentDictionary<InframanagerObject, EventPromise> _promises = 
            new ConcurrentDictionary<InframanagerObject, EventPromise>();

        private class EventPromise
        {
            public EventPromise(Event promiseObject) => Object = promiseObject;

            public EventPromise(Action<Event> firstAction) => Actions.Enqueue(firstAction);

            public Event Object { get; set; }
            public ConcurrentQueue<Action<Event>> Actions { get; } = new ConcurrentQueue<Action<Event>>();
        }

        public EventRepository(DbSet<Event> set, IDeleteStrategy<Event> deleteStrategy)
            : base(set, deleteStrategy)
        {
        }

        public void WhenAdded(InframanagerObject relatedObject, Action<Event> onEventAdded)
        {
            _promises.AddOrUpdate(
                relatedObject,
                new EventPromise(onEventAdded),
                (key, promise) =>
                {
                    if (promise.Object != null) // событие уже добавлено
                    {
                        onEventAdded(promise.Object);
                    }
                    else
                    {
                        promise.Actions.Enqueue(onEventAdded);
                    }

                    return promise;
                });
        }

        protected override void Inserted(Event entity)
        {
            foreach(var subject in entity.EventSubject.Where(subj => subj.ObjectId.HasValue && subj.ClassId.HasValue))
            {
                var relatedObject = new InframanagerObject(subject.ObjectId.Value, subject.ClassId.Value);
                _promises.AddOrUpdate(
                    relatedObject,
                    new EventPromise(entity),
                    (key, promise) =>
                    {
                        if (promise.Object == null) // только первый зарегистрированный ивент
                        {
                            promise.Object = entity;

                            Action<Event> action;
                            while (promise.Actions.TryDequeue(out action))
                            {
                                action(promise.Object);
                            }
                        }

                        return promise;
                    });
            }
        }
    }
}
