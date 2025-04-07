using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class IncidentResultsClient : ClientWithAuthorization
    {
        internal static string _url = "IncidentResults/";
        public IncidentResultsClient(string baseUrl) : base(baseUrl)
        {
        }

        //  TODO: Раскоментировать и поправить модели по готовности WebAPI
        public async Task<IncidentResultDetailsModel> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<IncidentResultDetailsModel>($"{_url}{guid}", userId, cancellationToken);
        }

        //public async Task<IncidentResultDetailsModel> SaveAsync(IncidentResultModel IncidentResult, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return await SaveAsync<IncidentResultDetailsModel, IncidentResultModel>(_url, IncidentResult, userId, cancellationToken);
        //}

        //public async Task<IncidentResultDetailsModel> AddAsync(IncidentResultModel IncidentResult, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return await AddAsync<IncidentResultDetailsModel, IncidentResultModel>(_url, IncidentResult, userId, cancellationToken);
        //}
        public async Task<IncidentResultListItemModel[]> GetList(ListFilter listFilter= null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListAsync<IncidentResultListItemModel[]>(_url, listFilter, userId, cancellationToken);
        }
    }
}
