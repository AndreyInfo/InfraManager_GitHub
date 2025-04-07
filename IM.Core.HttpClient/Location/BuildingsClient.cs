using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Location.Buildings;

namespace IM.Core.HttpClient.Location
{
    public class BuildingsClient : ClientWithAuthorization
    {
        private const string Path = "buildings";

        public BuildingsClient(string baseUrl) : base(baseUrl) { }


        public Task<BuildingDetails> GetAsync(int id, CancellationToken cancellationToken = default) =>
            GetAsync<BuildingDetails>($"{Path}/{id}", cancellationToken: cancellationToken);
    }
}
