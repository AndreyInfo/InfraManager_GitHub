using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.FormBuilder.Contracts;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class FullFormClient : ClientWithAuthorization
    {
        internal static string _url = "FullForms/";
        public FullFormClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<FormBuilderFullFormDetails> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<FormBuilderFullFormDetails>($"{_url}{guid}", userId, cancellationToken);
        }
    }
}