using InfraManager.CrossPlatform.WebApi.Contracts.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfraManager.CrossPlatform.WebApi.Infrastructure
{
    public static class LogFormatter
    {
        public static string WriteLog(this WebApiLogModel webApiLogModel)
        {
            return WriteIdentification(webApiLogModel) + Environment.NewLine +
                $"{nameof(WebApiLogModel.Query)}:{webApiLogModel.Query}{Environment.NewLine}" +
                $"{nameof(WebApiLogModel.RequestData)}:{webApiLogModel.RequestData}{Environment.NewLine}";
        }
        private static string WriteIdentification(WebApiLogModel webApiLogModel)
        {
            return $"{nameof(WebApiLogModel.Host)}:{webApiLogModel.Host}; {nameof(WebApiLogModel.Path)}:{webApiLogModel.Path}{Environment.NewLine}"+
                $"{nameof(WebApiLogModel.UserID)}:{webApiLogModel.UserID}; {nameof(WebApiLogModel.UserDeviceID)}:{webApiLogModel.UserDeviceID}; {nameof(WebApiLogModel.CorrelationID)}:{webApiLogModel.CorrelationID}";

        }
        public static string WriteLog(this WebApiLogResultModel webApiLogResultModel)
        {
            return WriteIdentification(webApiLogResultModel) + Environment.NewLine +
                $"{nameof(WebApiLogResultModel.Query)}:{webApiLogResultModel.Query}{Environment.NewLine}" +
                $"{nameof(WebApiLogResultModel.RequestData)}:{webApiLogResultModel.RequestData}{Environment.NewLine}"+
                $"{nameof(WebApiLogResultModel.ResultData)}:{webApiLogResultModel.ResultData}{Environment.NewLine}"
                ;
        }
    }
}
