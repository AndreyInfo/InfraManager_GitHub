using InfraManager.BLL;
using InfraManager.BLL.Messages;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.ServiceBase.MailService.WebAPIModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.Message
{
    public class EmailMessagesClient : ClientWithAuthorization
    {
        internal static string _url = "EmailMessages/";
        internal static string _settingUrl = "MailServiceSettings/";
        public EmailMessagesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<MessageByEmailDetails> AddAsync(MessageByEmailData messageByEmail, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<MessageByEmailDetails, MessageByEmailData>(_url, messageByEmail, userId, cancellationToken);
        }
        
        public async Task<MessageByEmailDetails> SaveAsync(Guid id, MessageByEmailData messsage, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PatchAsync<MessageByEmailDetails, MessageByEmailData>($"{_url}{id}", messsage, userId, cancellationToken);
        }
        
        public async Task<MessageByEmailDetails> GetAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<MessageByEmailDetails>($"{_url}{id}", userId, cancellationToken);
        }

        public async Task<byte[]> GetSettingValue(Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<byte[]>($"{_url}rules", userId, cancellationToken);
        }

        public async Task<MailServiceSettings> GetSettingsAsync(Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<MailServiceSettings>($"{_settingUrl}settings", userId, cancellationToken);
        }
    }
}
