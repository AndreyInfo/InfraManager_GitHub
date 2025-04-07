using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk.Calls;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class CallTypesClient : ClientWithAuthorization
    {
        internal static string _url = "CallTypes/";
        public CallTypesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<CallTypeDetails> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<CallTypeDetails>($"{_url}{guid}", userId, cancellationToken);
        }

        //  TODO: Раскоментировать и поправить модели по готовности WebAPI
        //public async Task<CallTypeDetailsModel> SaveAsync(CallTypeModel CallType, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return await SaveAsync<CallTypeDetailsModel, CallTypeModel>(_url, CallType, userId, cancellationToken);
        //}

        //public async Task<CallTypeDetailsModel> AddAsync(CallTypeModel CallType, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return await AddAsync<CallTypeDetailsModel, CallTypeModel>(_url, CallType, userId, cancellationToken);
        //}
        public async Task<CallTypeDetails[]> GetList(ListFilter listFilter= null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListAsync<CallTypeDetails[]>(_url, listFilter, userId, cancellationToken);
        }
    }
}
