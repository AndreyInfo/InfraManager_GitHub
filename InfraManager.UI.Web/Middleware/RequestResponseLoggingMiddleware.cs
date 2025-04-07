using InfraManager.CrossPlatform.WebApi.Contracts;
using InfraManager.CrossPlatform.WebApi.Contracts.Logging;
using InfraManager.UI.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IM.Core.HttpInfrastructure;

namespace InfraManager.CrossPlatform.WebApi.Infrastructure
{
    /// <summary>
    /// Логгер обращений к API
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Invoke(HttpContext context)
        {
            var webApiLogModel = await CreateLogRequestSafe(context.Request);

            if(webApiLogModel !=null)
                _logger.Log<WebApiLogModel>(LogLevel.Information, new EventId(Constants.WebApiRequestEventId, webApiLogModel.Path), webApiLogModel, null, (m, e) => $"{m.WriteLog()}{(e!=null?e.ToString():"")}");

            var stopwatch = Stopwatch.StartNew();

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await _next(context);
                    stopwatch.Stop();
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    var errorModel = CreateResultLogModel(webApiLogModel);
                    if (errorModel != null)
                    {
                        errorModel.ExecuteDuration = stopwatch.ElapsedMilliseconds;
                        errorModel.ExceptionData = ex.ToString();
                        errorModel.EventTime = DateTime.UtcNow;
                        _logger.Log<WebApiLogModel>(LogLevel.Error, new EventId(Constants.WebApiRequestEventId, webApiLogModel.Path), errorModel, ex, (m, e) => $"{m.WriteLog()}{(e != null ? e.ToString() : "")}");
                    }
                    throw;
                }

                var responseWebModel = await CreateLogResponseSafe(context.Response, webApiLogModel, stopwatch);
                if(responseWebModel!=null)
                    _logger.Log<WebApiLogResultModel>(LogLevel.Debug, new EventId(Constants.WebApiResponseEventId, webApiLogModel.Path), responseWebModel, null, (m, e) => $"{m.WriteLog()}{(e!=null?e.ToString():"")}");

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<WebApiLogModel> CreateLogRequestSafe(HttpRequest request)
        {
            string bodyAsText = null;
            WebApiLogModel logModel = null;
            try
            {
                request.EnableBuffering();

                var buffer = new byte[Convert.ToInt32(request.ContentLength)];

                await request.Body.ReadAsync(buffer, 0, buffer.Length);

                bodyAsText = Encoding.UTF8.GetString(buffer);

                logModel = ExtractLogInfo(request);
                logModel.RequestData = bodyAsText;

                request.Body.Seek(0, SeekOrigin.Begin);

                return logModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(RequestResponseLoggingMiddleware)}.{nameof(CreateLogRequestSafe)} : fail to extract data for logging");
                return logModel ?? ExtractLogInfo(request);
            }
        }

        private WebApiLogModel ExtractLogInfo(HttpRequest request)
        {
            var claimsIdentity = request.HttpContext?.User?.Identity as ClaimsIdentity;

            return request== null ? null : new()
            {
                CorrelationID = ExtractFromHeader(request.Headers, Constants.CorrelationIDHeader),
                Host = request.Host.ToString(),
                Path = request.Path,
                Query = request.QueryString.ToString(),
                UserDeviceID = ExtractFromHeader(request.Headers, Constants.UserDeviceIDHeader),
                UserID = claimsIdentity == null ? string.Empty : claimsIdentity.GetUserId(),
                EventTime = DateTime.UtcNow
            };
        }

        private async Task<WebApiLogResultModel> CreateLogResponseSafe(HttpResponse response, WebApiLogModel apiLogModel, Stopwatch stopwatch)
        {
            try
            {
                response.Body.Seek(0, SeekOrigin.Begin);

                string text = await new StreamReader(response.Body).ReadToEndAsync();

                response.Body.Seek(0, SeekOrigin.Begin);

                var logResult = CreateResultLogModel(apiLogModel);
                logResult.ResultData = text;
                logResult.ExecuteDuration = stopwatch.ElapsedMilliseconds;
                logResult.EventTime = DateTime.UtcNow;

                return logResult;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"{nameof(RequestResponseLoggingMiddleware)}.{nameof(CreateLogResponseSafe)} : fail to extract body");
                return CreateResultLogModel(apiLogModel ?? ExtractLogInfo(response?.HttpContext?.Request));
            }
        }

        private static WebApiLogResultModel CreateResultLogModel(WebApiLogModel apiLogModel)
        {
            return apiLogModel == null? null : new WebApiLogResultModel() 
            { 
                CorrelationID = apiLogModel.CorrelationID,
                EventTime = DateTime.UtcNow,
                Host = apiLogModel.Host,
                Path = apiLogModel.Path,
                Query = apiLogModel.Query,
                RequestData = apiLogModel.RequestData,
                Success = true,
                UserDeviceID = apiLogModel.UserDeviceID,
                UserID = apiLogModel.UserID,
            };
        }

        private string ExtractFromHeader(IHeaderDictionary headers, string headerName)
        {
            if (headers == null)
                return string.Empty;
            if (!headers.ContainsKey(headerName))
                return string.Empty;
            return headers[headerName].ToString();
        }
    }
}

