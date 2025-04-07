using InfraManager.BLL.Asset;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class TerminalDevicesClient : ClientWithAuthorization
    {
        internal static string _url = "terminalDevices/";
        public TerminalDevicesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<TerminalDeviceDetails> GetAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<TerminalDeviceDetails>($"{_url}{id}", userId, cancellationToken);
        }
    }
}
