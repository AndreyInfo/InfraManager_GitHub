using InfraManager.BLL.Location.Rooms;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class RoomsClient : ClientWithAuthorization
    {
        internal static string _url = "rooms/";
        public RoomsClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<RoomDetails> GetAsync(int id, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<RoomDetails>($"{_url}{id}", userId, cancellationToken);
        }
    }
}
