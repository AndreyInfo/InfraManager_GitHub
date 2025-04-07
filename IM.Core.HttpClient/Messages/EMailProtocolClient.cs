using InfraManager.WebApi.Contracts.Settings;
using InfraManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.WebApi.Contracts.Models.EMailProtocol;

namespace IM.Core.HttpClient.Messages
{
    public class EMailProtocolClient : ClientWithAuthorization
    {
        internal static string _url = "EMailProtocol/";
        public EMailProtocolClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<NotificationReceiverDetails[]> GetAsync(EMailListRequest request, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListByPostAsync<NotificationReceiverDetails[], EMailListRequest>(_url, request, userId, cancellationToken);
        }
    
    }
}
