using InfraManager.ServiceBase.TelephonyService.WebApiModels;
using InfraManager.Services;
using InfraManager.Services.TelephonyService;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.WebAPIClient
{
    public class TelephonyServiceClient : WebAPIBaseClient, ITelephonyService
    {
        public TelephonyServiceClient(string baseUrl)
       : base(baseUrl)
        {
        }

        public OperationResult CallTo(string fromNumber, string toNumber, bool outcommingCall, out bool result)
        {
            result = false;
            var task = PostAsync<TelephonyServiceCallToResult, TelephonyServiceCallToRequest>("telephonyservice/call-to", new TelephonyServiceCallToRequest()
            {
                FromNumber = fromNumber,
                ToNumber = toNumber,
                OutcommingCall = outcommingCall
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                var res = task.Result;
                result = res.callResult;
                return res.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call TelephonyService" };
        }

        public OperationResult EnsureAvailability()
        {
            var task = GetAsync<OperationResult>("telephonyservice/ensure");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call TelephonyService" };
        } 

        public OperationResult IsReady(out bool result)
        {
            result = false;
            var task = GetAsync<TelephonyServiceCallToResult>("telephonyservice/is-ready");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                var res = task.Result;
                result = res.callResult;
                return res.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult Subscribe(Guid applicationID)
        {
            var task = PostAsync<OperationResult, Guid>("telephonyservice/subscribe", applicationID);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call TelephonyService" };
        }

        public OperationResult Unsubscribe(Guid applicationID)
        {
            var task = PostAsync<OperationResult, Guid>("telephonyservice/unsubscribe", applicationID);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call TelephonyService" };
        }
    }
}
