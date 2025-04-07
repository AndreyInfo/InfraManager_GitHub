using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class PrioritiesClient : ClientWithAuthorization
    {
        internal static string _url = "priority/";
        public PrioritiesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<PriorityDetailsModel> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<PriorityDetailsModel>($"{_url}item?priorityID={guid}", userId, cancellationToken);
        }

        public async Task DeleteAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await DeleteAsync($"{_url}{guid}", userId, cancellationToken);
        }

        public async Task<PriorityDetailsModel> SaveAsync(Guid guid,PriorityModel Priority, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PutAsync<PriorityDetailsModel, PriorityModel>($"{_url}{guid}", Priority, userId, cancellationToken);
        }

        public async Task<PriorityDetailsModel> AddAsync(PriorityModel Priority, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<PriorityDetailsModel, PriorityModel>(_url, Priority, userId, cancellationToken);
        }

        public async Task<PriorityDetailsModel[]> GetListAsync(ListFilter listFilter = null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListAsync<PriorityDetailsModel[]>($"{_url}list", listFilter, userId, cancellationToken);
        }
    }
}
