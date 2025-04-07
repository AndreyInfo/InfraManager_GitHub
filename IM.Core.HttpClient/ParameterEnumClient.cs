using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Parameters;

namespace IM.Core.HttpClient
{
    public class ParameterEnumClient : ClientWithAuthorization
    {
        internal static string _url = "ParameterEnum/";

        public ParameterEnumClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<ParameterEnumValueData> GetValueAsync(Guid guid, Guid? userId = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<ParameterEnumValueData>($"{_url}parameterEnumValue?id={guid}", userId,
                cancellationToken);
        }
    }
}