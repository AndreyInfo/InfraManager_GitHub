using InfraManager.BLL.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient
{
    public class UriClient : ClientWithAuthorization
    {
        public UriClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<T> GetFromUriAsync<T>(string uri, Guid? userId = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<T>(uri, userId,
                cancellationToken);

        }
    }
}
