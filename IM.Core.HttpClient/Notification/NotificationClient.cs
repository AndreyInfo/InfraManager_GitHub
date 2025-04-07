using InfraManager.WebApi.Contracts.Settings;
using InfraManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Notification;

namespace IM.Core.HttpClient.Notification
{
    public class NotificationClient : ClientWithAuthorization
    {
        internal static string _url = "Notification/";
        public NotificationClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<NotificationDetails> GetAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<NotificationDetails>($"{_url}{id}", userId, cancellationToken);
        }
    }
}
