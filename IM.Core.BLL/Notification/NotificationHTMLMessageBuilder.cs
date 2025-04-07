using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk;
using InfraManager.Core;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Notification;

internal class NotificationHTMLMessageBuilder : INotificationHTMLMessageBuilder,
    ISelfRegisteredService<INotificationHTMLMessageBuilder>
{
    private readonly IFindEntityByGlobalIdentifier<User> _userFinder;
    private readonly IServiceDeskDateTimeConverter _timeConverted;

    public NotificationHTMLMessageBuilder(
        IFindEntityByGlobalIdentifier<User> userFinder,
        IServiceDeskDateTimeConverter serviceDeskDateTimeConverter)
    {
        _userFinder = userFinder;
        _timeConverted = serviceDeskDateTimeConverter;
    }
    
    public async Task<string> BuildMessageHTMLTextAsync(IEnumerable<Note> messages, CancellationToken cancellationToken)
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.Append("<br/><ul style=\"list-style-type:circle\">");
        foreach (var message in messages.OrderBy(x => x.UtcDate))
        {
            var user = await _userFinder.FindAsync(message.UserID, cancellationToken);

            sb.Append("<span>");
            sb.Append($"<li><b>{user?.FullName}</b> ({(await _timeConverted.ConvertAsync(message.UtcDate, cancellationToken)).ToString(Global.DateTimeFormat)})");
            sb.Append("<br/>");
            sb.Append($"{message.HTMLNote}</li>");
            sb.Append("</span>");
        }

        sb.Append("</ul>");
        return sb.ToString();
    }

    public async Task<string> BuildMessageTextAsync(IEnumerable<Note> messages, CancellationToken cancellationToken)
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        foreach (var message in messages.OrderBy(x => x.UtcDate))
        {
            var user = await _userFinder.FindAsync(message.UserID, cancellationToken);
            
            sb.AppendLine($"\t{user?.FullName}\t\t({(await _timeConverted.ConvertAsync(message.UtcDate, cancellationToken)).ToString(Global.DateTimeFormat)})");
            sb.AppendLine(message.NoteText);
            sb.AppendLine();
        }

        return sb.ToString();
    }
}