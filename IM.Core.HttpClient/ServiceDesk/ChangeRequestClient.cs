using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.ChangeRequests;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.ChangeRequest;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class ChangeRequestClient : ClientWithAuthorization
    {
        private const string Url = "changerequests/";

        public ChangeRequestClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<ChangeRequestDetailsModel> GetAsync(Guid guid,
            Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<ChangeRequestDetailsModel>($"{Url}{guid}", userId, cancellationToken);
        }

        public async Task<ChangeRequestDetailsModel> PutAsync(Guid callID,
            ChangeRequestDataModel changeRequest,
            Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            return await PatchAsync<ChangeRequestDetailsModel, ChangeRequestDataModel>($"{Url}{callID}", changeRequest, userId, cancellationToken);
        }

        public async Task<ChangeRequestDetailsModel> AddAsync(ChangeRequestData rfcResult, 
            Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            return await PostAsync<ChangeRequestDetailsModel, ChangeRequestData>(Url, rfcResult, userId, cancellationToken);
        }

        public async Task<DependencyDetails[]> GetDependenciesAsync(Guid rfcID, 
            Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<DependencyDetails[]>( $"{Url}{rfcID}/dependencies", userId, cancellationToken);
        }

        public async Task<ChangeRequestDetailsModel> GetResultAsync(Guid id, 
            Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<ChangeRequestDetailsModel>($"{Url}{id}", cancellationToken: cancellationToken);
        }
        
        public async Task<ChangeRequestDetailsModel[]> GetListAsync(ChangeRequestListFilter listFilter, 
            Guid? userId = null, 
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<ChangeRequestDetailsModel[], ChangeRequestListFilter>(Url, listFilter, x => PreProcessRequestHeaders(x, userId), cancellationToken);
        }
    }
}