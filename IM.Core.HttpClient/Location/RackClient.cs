using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Asset.Filters;
using InfraManager.BLL.Location.Racks;

namespace IM.Core.HttpClient.Location
{
    public class RackClient : ClientWithAuthorization
    {
        private const string Path = "rack";

        public RackClient(string baseUrl) : base(baseUrl) { }

        public Task<RackDetails> GetByIDAsync(int id, CancellationToken cancellationToken) =>
            GetAsync<RackDetails>($"{Path}/{id}", cancellationToken: cancellationToken);

        public Task<RackDetails[]> GetListAsync(RackListFilter filter, CancellationToken cancellationToken) =>
            GetListAsync<RackDetails[], RackListFilter>($"{Path}/list", filter, cancellationToken: cancellationToken);
    }
}
