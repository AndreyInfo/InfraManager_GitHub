using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Events
{
    internal class EventsForObjectQuery : IEventsForObjectQuery, ISelfRegisteredService<IEventsForObjectQuery>
    {
        private readonly CrossPlatformDbContext _db;

        public EventsForObjectQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<EventForObjectQueryResultItem[]> QueryAsync(EventsForObjectQueryRequest request, CancellationToken cancellationToken)
        {
            var parameters = request.Parameters ?? Array.Empty<string>();

            var query = _db.Set<Event>()
                .Include(@event => @event.User)
                .Include(@event => @event.EventSubject)
                .ThenInclude(subject => subject.EventSubjectParam)
                .AsNoTracking();

            if (request.ParentID.HasValue)
            {
                query = query.Where(@event => @event.ParentID == request.ParentID.Value);
            }
            else
            {
                query = query.Where(@event =>
                    @event.EventSubject.Any(subject =>
                        subject.ObjectId == request.ObjectID
                        && (!request.ClassID.HasValue
                            || subject.ClassId == request.ClassID)));
            }

            if (request.DateFrom.HasValue)
            {
                query = query.Where(@event => @event.Date >= request.DateFrom.Value);
            }

            if (request.DateTill.HasValue)
            {
                query = query.Where(@event => @event.Date <= request.DateTill.Value);
            }

            return await query
                .SelectMany(
                    @event => @event.EventSubject.DefaultIfEmpty(),
                    (@event, subject) => new { Event = @event, Subject = subject, })
                .SelectMany(
                    x => x.Subject.EventSubjectParam.DefaultIfEmpty(),
                    (x, param) => new { Event = x.Event, Subject = x.Subject, Param = param, })
                .Where(x => !parameters.Any() || parameters.Contains(x.Param.ParamName))
                .Select(x => new EventForObjectQueryResultItem
                {
                    ID = x.Event.Id,
                    UtcDate = x.Event.Date,
                    UserID = x.Event.User.IMObjID,
                    UserName = x.Event.User.FullName,
                    Message = x.Event.Message,
                    ParameterName = x.Param.ParamName,
                    ParameterOldValue = x.Param.ParamOldValue,
                    ParameterNewValue = x.Param.ParamNewValue,
                    Order = x.Event.InsertOrder,
                })
                .ToArrayAsync(cancellationToken: cancellationToken);
        }
    }
}
