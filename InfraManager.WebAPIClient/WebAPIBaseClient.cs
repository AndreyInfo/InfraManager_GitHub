using Inframanager;
using InfraManager.WebApi.Contracts.Models.Documents;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace InfraManager.WebAPIClient
{
    public class WebAPIBaseClient : HttpClient
    {
        public WebAPIBaseClient(string baseUrl)
        {
            if (!string.IsNullOrEmpty(baseUrl))
                BaseAddress = new Uri(baseUrl);
        }

        /// <summary>
        /// Исполнение запроса к серверу(без тела запроса) с получением ответа
        /// </summary>
        /// <typeparam name="TResponse">Тип запроса</typeparam>
        /// <param name="requestUrl">урл запроса</param>
        /// <param name="request">запрос</param>
        /// <returns></returns>
        protected async Task<TResponse> PostAsync<TResponse>(string requestUrl,
            Action<HttpRequestHeaders> preProcessHeader = null, CancellationToken cancellationToken = default)
        {
            return await SendRequestAsync<TResponse>(() =>
            {
                var req = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                preProcessHeader?.Invoke(req.Headers);
                return req;
            }, cancellationToken).ConfigureAwait(false);
        }
        
        
        /// <summary>
        /// Исполнение запроса к серверу с получением ответа
        /// </summary>
        /// <typeparam name="TResponse">Тип ответа</typeparam>
        /// <typeparam name="TRequest">Тип запроса</typeparam>
        /// <param name="requestUrl">урл запроса</param>
        /// <param name="request">запрос</param>
        /// <returns></returns>
        protected async Task<TResponse> PostAsync<TResponse, TRequest>(string requestUrl, TRequest request,
            Action<HttpRequestHeaders> preProcessHeader = null, CancellationToken cancellationToken = default)
        {
            return await SendRequestAsync<TResponse>(() =>
            {
                var req = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(request, JsonSettings), Encoding.UTF8, "application/json")
                };
                preProcessHeader?.Invoke(req.Headers);
                return req;
            }, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Исполнение запроса к серверу с получением ответа
        /// </summary>
        /// <typeparam name="TResponse">Тип ответа</typeparam>
        /// <typeparam name="TRequest">Тип запроса</typeparam>
        /// <param name="requestUrl">урл запроса</param>
        /// <param name="request">запрос</param>
        /// <returns></returns>
        protected async Task<TResponse> PutAsync<TResponse, TRequest>(string requestUrl, TRequest request,
            Action<HttpRequestHeaders> preProcessHeader = null, CancellationToken cancellationToken = default)
        {
            return await SendRequestAsync<TResponse>(() =>
            {
                var req = new HttpRequestMessage(HttpMethod.Put, requestUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(request, JsonSettings), Encoding.UTF8, "application/json")
                };
                preProcessHeader?.Invoke(req.Headers);
                return req;
            }, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Исполнение запроса к серверу с получением ответа
        /// </summary>
        /// <typeparam name="TResponse">Тип ответа</typeparam>
        /// <typeparam name="TRequest">Тип запроса</typeparam>
        /// <param name="requestUrl">урл запроса</param>
        /// <param name="request">запрос</param>
        /// <returns></returns>
        protected async Task<TResponse> PatchAsync<TResponse, TRequest>(string requestUrl, TRequest request,
            Action<HttpRequestHeaders> preProcessHeader = null, CancellationToken cancellationToken = default)
        {
            return await SendRequestAsync<TResponse>(() =>
            {
                var req = new HttpRequestMessage(HttpMethod.Patch, requestUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(request, JsonSettings), Encoding.UTF8, "application/json")
                };
                preProcessHeader?.Invoke(req.Headers);
                return req;
            }, cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Исполнение запроса к серверу с получением ответа
        /// </summary>
        /// <typeparam name="TRequest">Тип запроса</typeparam>
        /// <param name="requestUrl">урл запроса</param>
        /// <param name="request">запрос</param>
        /// <returns></returns>
        protected async Task PutAsync<TRequest>(string requestUrl, TRequest request,
            Action<HttpRequestHeaders> preProcessHeader = null, CancellationToken cancellationToken = default)
        {
            var req = new HttpRequestMessage(HttpMethod.Put, requestUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(request, JsonSettings), Encoding.UTF8, "application/json")
            };
            preProcessHeader?.Invoke(req.Headers);

            await SendRequestRawAsync(req, cancellationToken).ConfigureAwait(false);
        }

        protected async Task<TResponse> PutAsync<TResponse>(string requestUrl,
            Action<HttpRequestHeaders> preProcessHeader = null, CancellationToken cancellationToken = default)
        {
            return await SendRequestAsync<TResponse>(() =>
            {
                var req = new HttpRequestMessage(HttpMethod.Put, requestUrl);
                preProcessHeader?.Invoke(req.Headers);
                return req;
            }, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Исполнение запроса к серверу с получением ответа
        /// </summary>
        /// <typeparam name="TResponse">Тип ответа</typeparam>
        /// <typeparam name="TRequest">Тип запроса</typeparam>
        /// <param name="requestUrl">урл запроса</param>
        /// <param name="request">запрос</param>
        /// <returns></returns>
        protected async Task<TResponse> GetAsync<TResponse, TRequest>(string requestUrl, TRequest request,
            Action<HttpRequestHeaders> preProcessHeader = null, CancellationToken cancellationToken = default)
        {
            return await SendRequestAsync<TResponse>(() =>
            {
                var req = new HttpRequestMessage(HttpMethod.Get, requestUrl + (request==null ? "" : "?" + QueryString(request)))
                {
                };
                preProcessHeader?.Invoke(req.Headers);
                return req;
            }, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Исполнение запроса к серверу с получением ответа
        /// </summary>
        /// <typeparam name="TResponse">Тип ответа</typeparam>
        /// <param name="requestUrl">урл запроса</param>
        /// <param name="request">запрос</param>
        /// <returns></returns>
        protected async Task<TResponse> GetAsync<TResponse>(string requestUrl, string request,
            Action<HttpRequestHeaders> preProcessHeader = null, CancellationToken cancellationToken = default)
        {
            return await SendRequestAsync<TResponse>(() =>
            {
                var req = new HttpRequestMessage(HttpMethod.Get, requestUrl + (string.IsNullOrEmpty(request) ? "" : "?" + request))
                {
                };
                preProcessHeader?.Invoke(req.Headers);
                return req;
            }, cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Исполнение удаления на сервере с получением ответа
        /// </summary>
        /// <param name="requestUrl">урл запроса</param>
        /// <param name="request">запрос</param>
        /// <returns></returns>
        protected async Task DeleteAsync(string requestUrl, string request,
            Action<HttpRequestHeaders> preProcessHeader = null, CancellationToken cancellationToken = default)
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, requestUrl + (string.IsNullOrEmpty(request) ? "" : "?" + request))
            {
            };
            preProcessHeader?.Invoke(req.Headers);

            await SendRequestRawAsync(req, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Исполнение запроса к серверу с получением ответа
        /// </summary>
        /// <typeparam name="TResponse">Тип ответа</typeparam>
        /// <param name="requestUrl">урл запроса</param>
        /// <param name="request">запрос</param>
        /// <returns></returns>
        protected async Task<TResponse> GetAsync<TResponse>(string requestUrl,
            Action<HttpRequestHeaders> preProcessHeader = null, CancellationToken cancellationToken = default)
        {
            return await SendRequestAsync<TResponse>(() =>
            {
                var req = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                preProcessHeader?.Invoke(req.Headers);
                return req;
            }, cancellationToken).ConfigureAwait(false);
        }

        protected virtual JsonSerializerSettings JsonSettings { get; } = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            Converters = new JsonConverter[]
            {
                new NullableGuidPropertyConverter(),
                new NullableIntPropertyConverter()
            }
        };

        /// <summary>
        /// Отправляем запрос на сервере и получаем типизировнный ответ
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="getRequest"></param>
        /// <returns></returns>
        protected async Task<TResponse> SendRequestAsync<TResponse>(Func<HttpRequestMessage> getRequest,
            CancellationToken cancellationToken = default)
        {
            var request = getRequest();

            using (var response = await SendAsync(request, cancellationToken).ConfigureAwait(false))
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                if (response.IsSuccessStatusCode)                    
                    return await ReadResponse<TResponse>(response).ConfigureAwait(false);

                throw await GetResponseExceptionAsync(response).ConfigureAwait(false);
            }
        }

        public async Task<string> GetRawAsync(string url, CancellationToken cancellationToken = default)
        {
            return await SendRequestRawAsync(new HttpRequestMessage(HttpMethod.Get, url), cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> PutRawAsync(string url, string content, CancellationToken cancellationToken = default)
        {
            return await SendRequestRawAsync(
                    new HttpRequestMessage(HttpMethod.Put, url)
                    {
                        Content = new StringContent(
                            content,
                            Encoding.UTF8,
                            "application/json")
                    },
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<string> PostRawAsync(string url, string content, CancellationToken cancellationToken = default)
        {
            return await SendRequestRawAsync(
                    new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = new StringContent(
                            content,
                            Encoding.UTF8,
                            "application/json")
                    },
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<string> SendRequestRawAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken = default)
        {
            using (var response = await SendAsync(request, cancellationToken).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await GetResponseExceptionAsync(response).ConfigureAwait(false);
                }

                return await response.Content.ReadAsStringAsync();
            }
        }


        /// <summary>
        /// Получаем исключение из ответа сервера
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<Exception> GetResponseExceptionAsync(HttpResponseMessage message)
        {
            try
            {
                var stringContent = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (string.IsNullOrEmpty(stringContent))
                    switch(message.StatusCode)
                    {
                        case HttpStatusCode.Forbidden:
                            return new UnauthorizedAccessException($"{message.RequestMessage.RequestUri} forbidden");
                        case HttpStatusCode.NotFound:
                            return new ObjectNotExistsException($"{message.RequestMessage.RequestUri} not found");
                        default:
                            return new Exception($"Unhandled status code detected: {message.StatusCode} ({message.ReasonPhrase})");
                    }
                if (message.StatusCode is HttpStatusCode.UnprocessableEntity)
                {
                    return new UniqueKeyConstraintViolationException(stringContent);
                }
                return new Exception(stringContent);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Читаем ответ сервера и десериализуем
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<TResponse> ReadResponse<TResponse>(HttpResponseMessage response)
        {
            try
            {
                if (typeof(TResponse) == typeof(DocumentFileModel))
                {
                    var fileBytes = await response.Content.ReadAsByteArrayAsync();
                    var fileModel = new DocumentFileModel
                    {
                        Data = fileBytes,
                        ContentType = response.Content.Headers.ContentType.MediaType,
                        FileName = response.Content.Headers.ContentDisposition.FileName
                    };
                    //лишний boxing/unboxing, но иначе - не привести тип.
                    return (TResponse)(object)fileModel;
                }
                else
                {
                    switch (response.Content.Headers?.ContentType?.MediaType)
                    {
                        case "application/json":
                        case "text/plain":
                            var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            return JsonConvert.DeserializeObject<TResponse>(stringContent, JsonSettings);

                        default:
                            if (response.Content.Headers?.ContentType == null)
                            {
                                return default;
                            }
                            throw new NotSupportedException("Неподдерживаемый тип контента: " + response.Content.Headers.ContentType.MediaType);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"{response.StatusCode}: " +
                    $"Не удалось прочитать ответ сервера. ", ex);
            }
        }
        private string QueryString<TRequest>(TRequest request)
        {
            var values = ConvertToQueryStringCollection(request);
            return string.Join("&", values.AllKeys
                .SelectMany(values.GetValues, (key, value) => new { key, value })
                .Where(pair => !string.IsNullOrWhiteSpace(pair.value))
                .Select(pair => $"{HttpUtility.UrlEncode(pair.key)}={HttpUtility.UrlEncode(pair.value)}"));
        }

        private NameValueCollection ConvertToQueryStringCollection(object req)
        {
            var result = new NameValueCollection();

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(req))
            {
                if (descriptor == null)
                    continue;

                var value = descriptor.GetValue(req);
                if (value == null)
                    continue;

                if (value is IList list)
                {
                    foreach (var item in list)
                    {
                        result.Add(descriptor.Name, item?.ToString());
                    }
                }
                else
                {
                    result.Add(descriptor.Name, value.ToString());
                }
            }

            return result;
        }


        /// <summary>
        ///     Исполнение запроса к серверу(без тела запроса) с получением ответа
        /// </summary>
        /// <typeparam name="TResponse">Тип ответа</typeparam>
        /// <param name="requestUrl">урл запроса</param>
        /// <param name="preProcessHeader"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<TResponse> DeleteAsync<TResponse>(string requestUrl,
            Action<HttpRequestHeaders> preProcessHeader = null, CancellationToken cancellationToken = default)
        {
            return await SendRequestAsync<TResponse>(() =>
            {
                var req = new HttpRequestMessage(HttpMethod.Delete, requestUrl);
                preProcessHeader?.Invoke(req.Headers);
                return req;
            }, cancellationToken).ConfigureAwait(false);
        }
    }
}
