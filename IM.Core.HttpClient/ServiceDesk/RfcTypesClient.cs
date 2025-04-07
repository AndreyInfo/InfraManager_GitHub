using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk.ChangeRequests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class RfcTypesClient : ClientWithAuthorization
    {
        internal static string _url = "RfcTypes/";
        public RfcTypesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<RfcTypeDetailsModel> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<RfcTypeDetailsModel>($"{_url}{guid}", userId, cancellationToken);
        }

        public async Task<RfcTypeDetailsModel> SaveAsync(Guid guid, RfcTypeModel RfcType, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PutAsync<RfcTypeDetailsModel, RfcTypeModel>($"{_url}{guid}", RfcType, userId, cancellationToken);
        }

        public async Task<RfcTypeDetailsModel> AddAsync(RfcTypeModel RfcType, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<RfcTypeDetailsModel, RfcTypeModel>(_url, RfcType, userId, cancellationToken);
        }
        public async Task<RfcTypeListItemModel[]> GetList(ListFilter listFilter= null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListAsync<RfcTypeListItemModel[]>(_url, listFilter, userId, cancellationToken);
        }
    }
}
