using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class InfluencesClient : ClientWithAuthorization
    {
        internal static string _url = "Influences/";
        public InfluencesClient(string baseUrl) : base(baseUrl)
        {
        }

        //  TODO: Раскоментировать и поправить модели по готовности WebAPI
        public async Task<InfluenceDetailsModel> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<InfluenceDetailsModel>($"{_url}{guid}", userId, cancellationToken);
        }

        //public async Task<InfluenceDetailsModel> SaveAsync(InfluenceModel Influence, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return await SaveAsync<InfluenceDetailsModel, InfluenceModel>(_url, Influence, userId, cancellationToken);
        //}

        //public async Task<InfluenceDetailsModel> AddAsync(InfluenceModel Influence, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return await AddAsync<InfluenceDetailsModel, InfluenceModel>(_url, Influence, userId, cancellationToken);
        //}
        public async Task<InfluenceListItemModel[]> GetList(ListFilter listFilter= null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListAsync<InfluenceListItemModel[]>(_url, listFilter, userId, cancellationToken);
        }
    }
}
