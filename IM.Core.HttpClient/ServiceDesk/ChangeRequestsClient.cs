using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.ChangeRequests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class ChangeRequestsClient : ClientWithAuthorization
    {
        internal static string _url = "ChangeRequests/";
        public ChangeRequestsClient(string baseUrl) : base(baseUrl)
        {
        }

        //  TODO: Раскоментировать и поправить модели по готовности WebAPI
        //public async Task<ChangeRequestDetailsModel> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return await GetAsync<ChangeRequestDetailsModel>($"{_url}{guid}", userId, cancellationToken);
        //}

        //public async Task<ChangeRequestDetailsModel> SaveAsync(ChangeRequestModel ChangeRequest, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return await SaveAsync<ChangeRequestDetailsModel, ChangeRequestModel>(_url, ChangeRequest, userId, cancellationToken);
        //}

        //public async Task<ChangeRequestDetailsModel> AddAsync(ChangeRequestModel ChangeRequest, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return await AddAsync<ChangeRequestDetailsModel, ChangeRequestModel>(_url, ChangeRequest, userId, cancellationToken);
        //}
        public async Task<ChangeRequestListItem[]> GetList(ListFilter listFilter= null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListAsync<ChangeRequestListItem[]>(_url, listFilter, userId, cancellationToken);
        }

        public async Task<DependencyDetails[]> GetDependenciesAsync(Guid callID, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<DependencyDetails[]>(_url + $"{callID}/dependencies", userId, cancellationToken);
        }
    }
}
