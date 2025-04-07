using InfraManager.BLL.Messages;
using InfraManager.BLL.Settings;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using InfraManager.IM.BusinessLayer.Messages;
using InfraManager.ServiceBase.MailService.WebAPIModels;
using InfraManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InfraManager.UI.Web.Controllers.Api.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MailServiceSettingsController : ControllerBase
    {
        private IMailServerSettingsBLL _mailServerSettings;
        private IMessageByEmailBLL _messageByEmailBLL;

        public MailServiceSettingsController(IMailServerSettingsBLL mailServerSettings, IMessageByEmailBLL messageByEmailBLL)
        {
            _mailServerSettings = mailServerSettings;
            _messageByEmailBLL = messageByEmailBLL;
        }
        /// <summary>
        /// Возвращает настройки подключения к почтовому сервису
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("connectionSettings")]
        public async Task<ConnectionSettings> GetConnectionsSettingsAsync(CancellationToken cancellationToken = default)
        {
            return await _mailServerSettings.GetConnectionSettingsAsync(cancellationToken);
        }
        /// <summary>
        /// Сохраняет настройки подключения к почтовому сервису
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("connectionSettings")]
        public async Task SaveConnectionSettingsAsync([FromBody] ConnectionSettings settings, CancellationToken cancellationToken = default)
        {
            await _mailServerSettings.SaveConnectionSettingsAsync(settings, cancellationToken);
        }
        
        /// <summary>
        /// Возвращает настройки отправки почты
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("sendMailSettings")]
        public async Task<SendMailSettings> GetSendMailSettingsAsync(CancellationToken cancellationToken = default)
        {
            return await _mailServerSettings.GetSendMailSettingsAsync(cancellationToken);
        }
        /// <summary>
        /// Сохраняет настройки отправки почты
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("sendMailSettings")]
        public async Task SaveSendMailSettingsAsync([FromBody] SendMailSettings settings, CancellationToken cancellationToken = default)
        {
            await _mailServerSettings.SaveSendMailSettingsAsync(settings, cancellationToken);
        }

        /// <summary>
        /// Возвращает настройки получения почты
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("mailSettingsGetMail")]
        public async Task<GetMailSettings> GetMailSettingsAsync(CancellationToken cancellationToken = default)
        {
            return await _mailServerSettings.GetMailSettingsAsync(cancellationToken);
        }
        /// <summary>
        /// Сохраняет настройки получения почты
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("saveGetMailSettings")]
        public async Task SaveGetMailSettingsAsync([FromBody] GetMailSettings settings, CancellationToken cancellationToken = default)
        {
            await _mailServerSettings.SaveGetMailSettingsAsync(settings, cancellationToken);
        }

        /// <summary>
        /// Возвращает список значение для PolicyType
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("policyTypeValuesList")]
        public ListItem[] GetPolicyTypeValuesList()
        {
            return _mailServerSettings.GetPolicyTypeValuesList();
        }

        /// <summary>
        /// Возвращает список значение для Protocol
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("protocolValuesList")]
        public ListItem[] GetProtocolValuesList()
        {
            return _mailServerSettings.GetProlocolValuesList();
        }
       
        [HttpGet("processingRuleSet")]
        public async Task<MessageProcessingRuleSet<MessageByEmail>> GetProcessingRuleSetAsync(CancellationToken cancellationToken = default)
        {
            var bytes = await _messageByEmailBLL.GetRulesSettingsValueAsync(cancellationToken);
            var str = Encoding.UTF8.GetString(bytes);
            var xel = XElement.Parse(str);
            return new MessageProcessingRuleSet<MessageByEmail>(xel);
        }

        [HttpPost("processingRuleSet")]
        public async Task<OperationResult> SetProcessingRuleSetAsync([FromBody] List<MessageProcessingRule> messageProcessingRuleList, 
            CancellationToken cancellationToken = default)
        {
            return await _messageByEmailBLL.SetRulesSettingsValueAsync(messageProcessingRuleList, cancellationToken);
        }

        [HttpGet("emailQuoteTrimmers")]
        public async Task<EmailQuoteTrimmerData[]> GetEmailQuoteTrimmersDataAsync(CancellationToken cancellationToken = default)
        {
            return await _mailServerSettings.GetEmailQuoteTrimmersDataAsync(cancellationToken);
        }

        [HttpGet("emailQuoteTrimmer")]
        public async Task<EmailQuoteTrimmerData> GetEmailQuoteTrimmersDataAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _mailServerSettings.GetEmailQuoteTrimmerDataAsync(id, cancellationToken);
        }

        [HttpPost("emailQuoteTrimmer")]
        public async Task<Guid> AddEmailQuoteTrimmerAsync([FromBody] EmailQuoteTrimmerData data, CancellationToken cancellationToken = default)
        {
            return await _mailServerSettings.AddEmailQuoteTrimmerAsync(data, cancellationToken);
        }

        [HttpPut("emailQuoteTrimmer")]
        public async Task<Guid> UpdateEmailQuoteTrimmerAsync([FromBody] EmailQuoteTrimmerData data, CancellationToken cancellationToken = default)
        {
            return await _mailServerSettings.UpdateEmailQuoteTrimmerAsync(data, cancellationToken);
        }

        [HttpDelete("emailQuoteTrimmer")]
        public async Task DeleteEmailQuoteTrimmerAsync([FromBody] Guid id, CancellationToken cancellationToken = default)
        {
            await _mailServerSettings.DeleteEmailQuoteTrimmerAsync(id, cancellationToken);
        }

        /// <summary>
        /// Возвращает список разрешений
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("exceptions")]
        public async Task<PolicySettings> GetPolicyTypeListAsync(CancellationToken cancellationToken = default)
        {
            return await _mailServerSettings.GetPolicyTypeListAsync(cancellationToken);
        }
        /// <summary>
        /// Сохраняет список разрешений
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("exceptions")]
        public async Task SavePolicyTypeListAsync([FromBody] PolicySettings settings, CancellationToken cancellationToken = default)
        {
            await _mailServerSettings.SavePolicyTypeListAsync(settings, cancellationToken);
        }

        [HttpPost("testSMTPSettings")]
        public OperationResult TestSMTPSettings([FromBody] SMTPSettingsTest testData)
        {
            return _mailServerSettings.TestSMTPSettings(testData);
        }
        [HttpPost("testInboundProtocolSettings")]
        public OperationResult TestInboundProtocolSettings([FromBody] POPSettingsTest testData)
        {
            return _mailServerSettings.TestInboundProtocolSettings(testData);
        }

        [HttpGet("htmlTagWorkers")]
        public async Task<HtmlTagWorkerDetail[]> GetHtmlTagWorkersByQuoteTrimmerIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _mailServerSettings.GetHtmlTagWorkerByQuoteTrimmerIdAsync(id, cancellationToken);
        }

        //TODO фронтам все еще нужны такие запросы, убрать когда решат эту проблему
        [HttpPost("htmlTagWorkersByFilter")]
        public HtmlTagWorkerDetail[] GetHtmlTagWorkersByQuoteTrimmerId([FromBody]HtmlTagWorkerFilter filter)
        {
            return _mailServerSettings.GetHtmlTagWorkerByQuoteTrimmerId(filter);
        }

        [HttpGet("htmlTagWorker")]
        public async Task<HtmlTagWorkerDetail> GetHtmlTagWorkerAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _mailServerSettings.GetHtmlTagWorkerAsync(id, cancellationToken);
        } 

        [HttpPost("htmlTagWorker")]
        public async Task<Guid> AddHtmlTagWorkerAsync([FromBody] HtmlTagWorkerDetail data, CancellationToken cancellationToken = default)
        {
            return await _mailServerSettings.AddHtmlTagWorkerAsync(data, cancellationToken);
        }

        [HttpPut("htmlTagWorker")]
        public async Task<Guid> UpdateHtmlTagWorkerAsync([FromBody] HtmlTagWorkerDetail data, CancellationToken cancellationToken = default)
        {
            return await _mailServerSettings.UpdateHtmlTagWorkerAsync(data, cancellationToken);
        }

        [HttpDelete("htmlTagWorker")]
        public async Task DeleteHtmlTagWorkerAsync([FromBody] Guid id, CancellationToken cancellationToken = default)
        {
            await _mailServerSettings.DeleteHtmlTagWorkerAsync(id, cancellationToken);
        }

        [HttpGet("citateTrimmerUsing")]
        public async Task<bool> GetCitateTrimmerUsingAsync(CancellationToken cancellationToken = default)
        {
            return await _messageByEmailBLL.GetCitateTrimmerUsingAsync(cancellationToken);
        }

        [HttpPost("citateTrimmerUsing")]
        public async Task<OperationResult> SetCitateTrimmerUsingAsync(bool citateTrimmerUsing,
            CancellationToken cancellationToken = default)
        {
            return await _messageByEmailBLL.SetCitateTrimmerUsingAsync(citateTrimmerUsing, cancellationToken);
        }

        [HttpPost("htmlTagWorkers")]
        public async Task AddHtmlTagWorkersAsync([FromBody] HtmlTagWorkerDetail[] data,
           CancellationToken cancellationToken = default)
        {
            await _mailServerSettings.AddHtmlTagWorkersAsync(data, cancellationToken);
        }
        [HttpGet("settings")]
        public async Task<MailServiceSettings> GetSettingsAsync(CancellationToken cancellationToken = default)
        {
            return await _mailServerSettings.GetMailSettingsValueAsync(cancellationToken);
        }
    }
}
