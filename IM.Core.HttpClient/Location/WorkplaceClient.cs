using System.Threading.Tasks;
using System.Threading;
using InfraManager.BLL.Location.Workplaces;

namespace IM.Core.HttpClient.Location
{
    public class WorkplaceClient : ClientWithAuthorization
    {
        private const string Path = "workplaces";

        public WorkplaceClient(string baseUrl) : base(baseUrl) { }


        public Task<WorkplaceDetails> GetByIDAsync(int id, CancellationToken cancellationToken) =>
            GetAsync<WorkplaceDetails>($"{Path}/{id}", cancellationToken: cancellationToken);

        public Task<WorkplaceDetails[]> GetListAsync(
            WorkplaceListFilter filter, CancellationToken cancellationToken) =>
                GetListAsync<WorkplaceDetails[], WorkplaceListFilter>(
                    $"{Path}/list", filter, cancellationToken: cancellationToken);
    }
}
