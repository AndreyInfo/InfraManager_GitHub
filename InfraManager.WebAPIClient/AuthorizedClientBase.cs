using Microsoft.Net.Http.Headers;
using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.Core;
using InfraManager.Core.Logging;

namespace InfraManager.WebAPIClient
{
    public class ClientWithAuthorizationBase : WebAPIBaseClient
    {
        public ClientWithAuthorizationBase(string baseUrl) : base(baseUrl)
        {
        }

        protected async Task<TResponse> PutAsync<TResponse, TRequest>(string requestUrl, TRequest request,
            Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await base.PutAsync<TResponse, TRequest>(requestUrl, request,
                x => PreProcessRequestHeaders(x, userId), cancellationToken);
        }

        protected async Task<TResponse> PatchAsync<TResponse, TRequest>(string requestUrl, TRequest request,
            Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await base.PatchAsync<TResponse, TRequest>(requestUrl, request,
                x => PreProcessRequestHeaders(x, userId), cancellationToken);
        }

        protected async Task<TResponse> PutAsync<TResponse>(string requestUrl, Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            return await base.PutAsync<TResponse>(requestUrl, x => PreProcessRequestHeaders(x, userId),
                cancellationToken);
        }

        protected async Task<TResponse> PostAsync<TResponse, TRequest>(string requestUrl, TRequest request,
            Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await PostAsync<TResponse, TRequest>(requestUrl, request, x => PreProcessRequestHeaders(x, userId),
                cancellationToken);
        }

        protected async Task<TResponse> GetAsync<TResponse>(string requestUrl, Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            return await base.GetAsync<TResponse>(requestUrl, x => PreProcessRequestHeaders(x, userId),
                cancellationToken);
        }

        protected async Task<TResponse> GetAsync<TResponse, TRequest>(string requestUrl, TRequest request,
            Guid? userID = null, CancellationToken cancellationToken = default)
        {
            return await base.GetAsync<TResponse, TRequest>(requestUrl, request,
                x => PreProcessRequestHeaders(x, userID), cancellationToken);
        }

        protected async Task DeleteAsync(string requestUrl, Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            await base.DeleteAsync(requestUrl, null, x => PreProcessRequestHeaders(x, userId), cancellationToken);
        }
        protected async Task<TResponse> DeleteAsync<TResponse>(string requestUrl, Guid? userId = null,
           CancellationToken cancellationToken = default)
        {
            return await base.DeleteAsync<TResponse>(requestUrl, x => PreProcessRequestHeaders(x, userId), cancellationToken);
        }
        protected void PreProcessRequestHeaders(HttpRequestHeaders request, Guid? userId)
        {
            if (!string.IsNullOrWhiteSpace(ApplicationManager.Instance.WebAPISecret))
                try
                {
                    var value = TokenUtility.CreateToken(ApplicationManager.Instance.WebAPISecret,
                        userId ?? ApplicationManager.Instance.WebAPIUserId);

                    request.Add(HeaderNames.Authorization, $"{TokenAuthenticationScheme.SchemeName} {value}");
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Ошибка при формировании токена.");
                }
        }
    }
}
