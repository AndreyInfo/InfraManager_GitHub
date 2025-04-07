using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Calls;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class CallsClient : ClientWithAuthorization
    {
        private const string Url = "calls/";

        public CallsClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<CallDetailsModel> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<CallDetailsModel>($"{Url}{guid}", userId, cancellationToken);
        }

        public async Task<CallDetailsModel> PutAsync(Guid callID, CallData call, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PutAsync<CallDetailsModel, CallData>($"{Url}{callID}", call, userId, cancellationToken);
        }

        public async Task<CallDetailsModel> AddAsync(CallData call, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<CallDetailsModel, CallData>($"{Url}?isClientForm=false", call, userId, cancellationToken);
        }
        public async Task<CallListItem[]> GetListAsync(ListFilter listFilter= null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<CallListItem[], ListFilter>(Url + "reports/allcalls", listFilter, x => PreProcessRequestHeaders(x, userId), cancellationToken);
        }

        public async Task<CallDetailsModel[]> GetListAsync(CallListFilter listFilter= null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<CallDetailsModel[], CallListFilter>(Url, listFilter, x => PreProcessRequestHeaders(x, userId), cancellationToken);
        }

        public async Task<NoteListItemModel[]> GetNotesAsync(Guid callID, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<NoteListItemModel[]>(Url + $"{callID}/notes", userId, cancellationToken);
        }

        public async Task<DependencyDetails[]> GetDependenciesAsync(Guid callID, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<DependencyDetails[]>(Url + $"{callID}/dependencies", userId, cancellationToken);
        }
    }
}
