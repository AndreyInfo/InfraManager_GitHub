using InfraManager.DAL.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace InfraManager.BLL.Events
{
    internal class HistoryDataProvider : IHistoryDataProvider, ISelfRegisteredService<IHistoryDataProvider>
    {
        private readonly IEventsForObjectQuery _query;

        public HistoryDataProvider(IEventsForObjectQuery query)
        {
            _query = query;
        }

        public async Task<HistoryItemDetails[]> GetHistoryByObjectAsync(Guid objectID, ObjectClass? classID, HistoryFilter filter, CancellationToken cancellationToken = default)
        {
            var resultList = await GetHistoryByRequestAsync(
                new EventsForObjectQueryRequest
                {
                    ObjectID = objectID,
                    ClassID = classID,
                    DateFrom = filter.DateFrom,
                    DateTill = filter.DateTill,
                    Parameters = filter.Parameters,
                }, cancellationToken);

            return resultList.ToArray();
        }

        private async Task<IEnumerable<HistoryItemDetails>> GetHistoryByRequestAsync(
            EventsForObjectQueryRequest request,
            CancellationToken cancellationToken)
        {
            // todo: Все что выполняется в этом методе должно выполняться на клиенте.
            var result = new List<HistoryItemDetails>();

            var items = await _query.QueryAsync(request, cancellationToken);
            foreach (var group in items.GroupBy(x => x.ID))
            {
                var @event = group.First();
                if (request.Parameters == null || !request.Parameters.Any())
                {
                    result.Add(
                        new HistoryItemDetails
                        {
                            ID = @event.ID,
                            UserID = @event.UserID,
                            UserName = @event.UserName?.Trim() ?? string.Empty,
                            UtcDate = @event.UtcDate,
                            Message = @event.Message,
                            Order = @event.Order,
                        });
                }

                var eventDetails = group
                    .Where(x => !string.IsNullOrWhiteSpace(x.ParameterName))
                    .Where(x => !(string.IsNullOrWhiteSpace(x.ParameterOldValue) && string.IsNullOrWhiteSpace(x.ParameterNewValue)))
                    .ToArray();

                if (eventDetails.Any())
                {
                    result.Add(
                        new HistoryItemDetails
                        {
                            ID = @event.ID,
                            UserID = @event.UserID,
                            UserName = @event.UserName?.Trim() ?? string.Empty,
                            UtcDate = @event.UtcDate,
                            Message = BuildMessage(eventDetails),
                            Order = @event.Order,
                        });
                }

                result.AddRange(
                    await GetHistoryByRequestAsync(
                        new EventsForObjectQueryRequest
                        {
                            DateFrom = request.DateFrom,
                            DateTill = request.DateTill,
                            Parameters = request.Parameters,
                            ParentID = @event.ID,
                        },
                        cancellationToken));
            }

            return result
                .OrderByDescending(x => x.UtcDate)
                .ThenByDescending(x => x.Order)
                .ToArray();
        }

        private static string BuildMessage(IEnumerable<EventForObjectQueryResultItem> items)
        {
            var message = new StringBuilder();

            foreach (var parameter in items)
            {
                if (message.Length > 0)
                {
                    message.AppendLine();
                }

                message.Append(parameter.ParameterName).Append(':');
                if (!string.IsNullOrWhiteSpace(parameter.ParameterOldValue))
                {
                    message.Append(' ').Append(parameter.ParameterOldValue).Append(" →");
                }
                message.Append(' ');
                if (!string.IsNullOrWhiteSpace(parameter.ParameterNewValue))
                {
                    message.Append(parameter.ParameterNewValue);
                }
            }

            return message.ToString();
        }
    }
}
