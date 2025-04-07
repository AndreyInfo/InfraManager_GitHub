using InfraManager.BLL;
using InfraManager.BLL.Messages;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.ServiceBase.MailService.WebAPIModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.Message
{
    public class MessagesClient : ClientWithAuthorization
    {
        internal static string _url = "Messages/";
        public MessagesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<MessageDetails> SaveAsync(Guid id, MessageData messsage, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PatchAsync<MessageDetails, MessageData>($"{_url}{id}", messsage, userId, cancellationToken);
        }

        public async Task<MessageDetails> GetAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<MessageDetails>($"{_url}{id}", userId, cancellationToken);
        }
    }
}
