using InfraManager.BLL;
using InfraManager.BLL.Settings.TableFilters;
using InfraManager.WebAPIClient;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient
{
    public class ClientWithAuthorization : ClientWithAuthorizationBase
    {
        public ClientWithAuthorization(string baseUrl) : base(baseUrl)
        {
        }

        protected async Task<TResponse> GetListAsync<TResponse>(string requestUrl, ListFilter listFilter, Guid? userID = null, CancellationToken cancellationToken = default)
        {
            return await base.GetAsync<TResponse, ListFilter>(requestUrl, listFilter, (x) => PreProcessRequestHeaders(x, userID), cancellationToken);
        }
        
        protected async Task<TResponse> GetListAsync<TResponse, TFilter>(string requestUrl, TFilter listFilter, Guid? userID = null, CancellationToken cancellationToken = default)
        {
            return await base.GetAsync<TResponse, TFilter>(requestUrl, listFilter, (x) => PreProcessRequestHeaders(x, userID), cancellationToken);
        }

        protected async Task<TResponse> GetListAsync<TResponse>(string requestUrl, Guid? userID = null, CancellationToken cancellationToken = default)
        {
            return await base.GetAsync<TResponse>(requestUrl, (x) => PreProcessRequestHeaders(x, userID), cancellationToken);
        }
        
        protected async Task<TResponse> GetListByPostAsync<TResponse, TRequest>(string requestUrl, TRequest listFilter, Guid? userID = null, CancellationToken cancellationToken = default)
        {
            return await base.PostAsync<TResponse, TRequest>(requestUrl, listFilter, (x) => PreProcessRequestHeaders(x, userID), cancellationToken);
        }
    }
}
