using InfraManager.BLL.Asset.NetworkDevices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.Assets
{
    public class NetworkDevicesClient : ClientWithAuthorization
    {
        internal static string _url = "networkDevices/";
        public NetworkDevicesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<NetworkDeviceDetails> GetAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<NetworkDeviceDetails>($"{_url}{id}", userId, cancellationToken);
        }
    }
}
