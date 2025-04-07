using InfraManager.ComponentModel;
using InfraManager.Core.Data;
using InfraManager.ServiceBase.MailService;
using InfraManager.ServiceBase.MailService.WebAPIModels;
using InfraManager.ServiceBase.SearchService.WebAPIModels;
using InfraManager.ServiceBase.WorkflowService.WebAPIModels;
using InfraManager.Services;
using InfraManager.Services.MailService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.WebAPIClient
{
    public class MailServiceClient : WebAPIBaseClient, IMailService, IMailServiceApi
    {

        public MailServiceClient(string baseUrl)
            : base(baseUrl)
        {
        }

        #region IMailServiceApi

        public async Task<bool> EnsureAsync(CancellationToken cancellationToken = default)
        {

            try
            {
                var result = await GetAsync<OperationResult>(
                                         "/Mailservice/Ensure",
                                         preProcessHeader: null,
                                         cancellationToken: cancellationToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
        
        public OperationResult EnsureAvailability()
        {
            var task = GetAsync<OperationResult>("mailservice/ensure");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }

        public OperationResult GetMaxAttachmentSize(out int? maxSize)
        {
            var task = GetAsync<MailServiceResultModel>("mailservice/max-attachment-size");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                maxSize = task.Result.intValue;
                return task.Result.OperationResult;
            }
            maxSize = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }

        public OperationResult GetSMTPPort(out int port)
        {
            var task = GetAsync<MailServiceResultModel>("mailservice/smtp-port");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                port = task.Result.intValue ?? 0;
                return task.Result.OperationResult;
            }
            port = 0;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }

        public OperationResult GetSMTPServer(out string server)
        {
            var task = GetAsync<MailServiceResultModel>("mailservice/smtp-server");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                server = task.Result.stringValue;
                return task.Result.OperationResult;
            }
            server = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }

        public OperationResult SendMail(SMTPMessage message)
        {
            var task = PostAsync<OperationResult, MessageModel>("mailservice/send", new MessageModel(message));
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }

        public OperationResult Subscribe(Guid applicationID)
        {
            var task = PostAsync<OperationResult, Guid>("mailservice/subscribe", applicationID);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }

        public OperationResult Unsubscribe(Guid applicationID)
        {
            var task = PostAsync<OperationResult, Guid>("mailservice/unsubscribe", applicationID);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }

        public OperationResult GetConnectionSettings(out ConnectionSettings settings)
        {
            var task = GetAsync<ConnectionSettings>("mailservice/getConnectionSettings");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                settings = task.Result;
                return new OperationResult()
                {
                    Type = OperationResultType.Success
                };
            }
            settings = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }
        public OperationResult SaveConnectionSettings(ConnectionSettings settings)
        {
            var task = PostAsync<OperationResult, ConnectionSettings>("mailservice/saveConnectionSettings", settings);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }
        public OperationResult GetSendMailSettings(out SendMailSettings settings)
        {
            var task = GetAsync<SendMailSettings>("mailservice/getSendMailSettings");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                settings = task.Result;
                return new OperationResult()
                {
                    Type = OperationResultType.Success
                };
            }
            settings = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }
        public OperationResult SaveSendMailSettings(SendMailSettings settings)
        {
            var task = PostAsync<OperationResult, SendMailSettings>("mailservice/saveSendMailSettings", settings);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }
        public OperationResult GetMailSettings(out GetMailSettings settings)
        {
            var task = GetAsync<GetMailSettings>("mailservice/getMailSettings");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                settings = task.Result;
                return new OperationResult()
                {
                    Type = OperationResultType.Success
                };
            }
            settings = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }
        public OperationResult SaveMailSettings(GetMailSettings settings)
        {
            var task = PostAsync<OperationResult, GetMailSettings>("mailservice/saveMailSettings", settings);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }
        public OperationResult GetPolicyTypeList(out PolicySettings settings)
        {
            var task = GetAsync<PolicySettings>("mailservice/getPolicyTypeList");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                settings = task.Result;
                return new OperationResult()
                {
                    Type = OperationResultType.Success
                };
            }
            settings = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }
        public OperationResult SavePolicyTypeList(PolicySettings settings)
        {
            var task = PostAsync<OperationResult, PolicySettings>("mailservice/savePolicyTypeList", settings);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call MailService" };
        }

        public OperationResult TestInboundProtocolSettings(POPSettingsTest testData)
        {
            var task = PostAsync<OperationResult, POPSettingsTest>("mailservice/testInboundProtocolSettings", testData);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Ошибка соединения" };
        }

        public OperationResult TestSMTPSettings(SMTPSettingsTest testData)
        {
            var task = PostAsync<OperationResult, SMTPSettingsTest>("mailservice/testSMTPSettings", testData);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Ошибка соединения" };
        }
    }
}
