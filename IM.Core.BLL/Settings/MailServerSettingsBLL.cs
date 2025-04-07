
using AutoMapper;
using InfraManager.BLL.Extensions;
using InfraManager.BLL.Settings.MailService;
using InfraManager.Core;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using InfraManager.DAL;
using InfraManager.DAL.Settings;
using InfraManager.ServiceBase.MailService.WebAPIModels;
using InfraManager.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    public class MailServerSettingsBLL : IMailServerSettingsBLL, ISelfRegisteredService<IMailServerSettingsBLL>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<EmailQuoteTrimmer> _repository;
        private readonly IRepository<HtmlTagWorker> _htmlTagWorkerRepository;
        private readonly IFinder<EmailQuoteTrimmer> _finder;
        private readonly IFinder<HtmlTagWorker> _htmlTagWorkerFinder;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IFinder<Setting> _settingsFinder;
        private readonly IRepository<Setting> _settingsRepository;
        private readonly ISettingsBLL _settingsBLL;
        private readonly IMemoryCache _cache;
        public MailServerSettingsBLL(IRepository<EmailQuoteTrimmer> repository,
            IMapper mapper,
            IUnitOfWork saveChangesCommand,
            IFinder<EmailQuoteTrimmer> finder,
            IRepository<HtmlTagWorker> htmlTagWorkerRepository,
            IFinder<HtmlTagWorker> htmlTagWorkerFinder,
            IFinder<Setting> settingsFinder,
            IRepository<Setting> settingsRepository,
            ISettingsBLL settingsBLL,
            IMemoryCache cache
            )
        {
            _repository = repository;
            _mapper = mapper;
            _saveChangesCommand = saveChangesCommand;
            _finder = finder;
            _htmlTagWorkerRepository = htmlTagWorkerRepository;
            _htmlTagWorkerFinder = htmlTagWorkerFinder;
            _settingsFinder = settingsFinder;
            _settingsRepository = settingsRepository;
            _settingsBLL = settingsBLL;
            _cache = cache;
        }
        //TODO перенести функционал в сам MailService
        public async Task<ConnectionSettings> GetConnectionSettingsAsync(CancellationToken cancellationToken = default)
        {
            var mailSettings = await GetMailSettingsValueAsync(cancellationToken);
            return _mapper.Map<ConnectionSettings>(mailSettings);
        }
        public async Task SaveConnectionSettingsAsync(ConnectionSettings settings, CancellationToken cancellationToken = default)
        {
            var mailSettings = await GetMailSettingsValueAsync(cancellationToken);
            _mapper.Map(settings, mailSettings);
            await SetMailSettingsValueAsync(mailSettings, cancellationToken);
        }

        public async Task<SendMailSettings> GetSendMailSettingsAsync(CancellationToken cancellationToken = default)
        {
            var mailSettings = await GetMailSettingsValueAsync(cancellationToken);
            return _mapper.Map<SendMailSettings>(mailSettings);
        }
        public async Task SaveSendMailSettingsAsync(SendMailSettings settings, CancellationToken cancellationToken = default)
        {
            var mailSettings = await GetMailSettingsValueAsync(cancellationToken);
            _mapper.Map(settings, mailSettings);
            await SetMailSettingsValueAsync(mailSettings, cancellationToken);
        }

        public ListItem[] GetProlocolValuesList()
        {
            return EnumExtensions.GetEnumListFriendlyName<Protocol>().ToArray();
        }
        public ListItem[] GetPolicyTypeValuesList()
        {
            return EnumExtensions.GetEnumListFriendlyName<PolicyType>().ToArray();
        }
        public async Task<GetMailSettings> GetMailSettingsAsync(CancellationToken cancellationToken = default)
        {
            var mailSettings = await GetMailSettingsValueAsync(cancellationToken);
            return _mapper.Map<GetMailSettings>(mailSettings);
        }
        public async Task SaveGetMailSettingsAsync(GetMailSettings settings, CancellationToken cancellationToken = default)
        {
            var mailSettings = await GetMailSettingsValueAsync(cancellationToken);
            _mapper.Map(settings, mailSettings);
            await SetMailSettingsValueAsync(mailSettings, cancellationToken);
        }
        public async Task<PolicySettings> GetPolicyTypeListAsync(CancellationToken cancellationToken = default)
        {
            var mailSettings = await GetMailSettingsValueAsync(cancellationToken);
            return _mapper.Map<PolicySettings>(mailSettings);
        }
        public async Task SavePolicyTypeListAsync(PolicySettings settings, CancellationToken cancellationToken = default)
        {
            var mailSettings = await GetMailSettingsValueAsync(cancellationToken);
            _mapper.Map(settings, mailSettings);
            await SetMailSettingsValueAsync(mailSettings, cancellationToken);
        }

        public OperationResult TestSMTPSettings(SMTPSettingsTest settings)
        {
            return MailManager.TestSMTPSettings(settings);
        }

        public OperationResult TestInboundProtocolSettings(POPSettingsTest settings)
        {
            return MailManager.TestInboundProtocolSettings(settings);
        }

        public async Task<EmailQuoteTrimmerData[]> GetEmailQuoteTrimmersDataAsync(CancellationToken cancellationToken = default)
        {
            var emailQuoteTrimmers = await _repository.ToArrayAsync(cancellationToken);
            return _mapper.Map<EmailQuoteTrimmerData[]>(emailQuoteTrimmers);
        }

        public async Task<EmailQuoteTrimmerData> GetEmailQuoteTrimmerDataAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var emailQuoteTrimmer = await _finder.FindAsync(id, cancellationToken);
            return _mapper.Map<EmailQuoteTrimmerData>(emailQuoteTrimmer);
        }

        public async Task<Guid> AddEmailQuoteTrimmerAsync(EmailQuoteTrimmerData data,
           CancellationToken cancellationToken = default)
        {
            var emailQuoteTrimmer = _mapper.Map<EmailQuoteTrimmer>(data);
            _repository.Insert(emailQuoteTrimmer);
            await _saveChangesCommand.SaveAsync(cancellationToken);
            return emailQuoteTrimmer.ID;
        }

        public async Task<Guid> UpdateEmailQuoteTrimmerAsync(EmailQuoteTrimmerData data,
          CancellationToken cancellationToken = default)
        {
            var item = await _finder.FindAsync(data.ID, cancellationToken);
            _mapper.Map(data, item);
            await _saveChangesCommand.SaveAsync(cancellationToken);
            return item.ID;
        }

        public async Task DeleteEmailQuoteTrimmerAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            var entity = await _finder.FindAsync(id, cancellationToken);
            _repository.Delete(entity);
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task<HtmlTagWorkerDetail[]> GetHtmlTagWorkerByQuoteTrimmerIdAsync(Guid emailQuoteTrimmerId, CancellationToken cancellationToken = default)
        {
            var htmlTagWorkers = await _htmlTagWorkerRepository.ToArrayAsync(p => p.QuoteTrimmerID == emailQuoteTrimmerId, cancellationToken);
            return _mapper.Map<HtmlTagWorkerDetail[]>(htmlTagWorkers);
        }

        public HtmlTagWorkerDetail[] GetHtmlTagWorkerByQuoteTrimmerId(HtmlTagWorkerFilter filter)
        {
            var query = _htmlTagWorkerRepository.Query().Where(p => p.QuoteTrimmerID == filter.EmailQuoteTrimmerId);
            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(x => x.Name.ToLower().Equals(filter.SearchString.ToLower()));
            }
            var tags = query.Skip(filter.StartRecordIndex).Take(filter.CountRecords).ToArray();
            return _mapper.Map<HtmlTagWorkerDetail[]>(tags);
        }

        public async Task<HtmlTagWorkerDetail> GetHtmlTagWorkerAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var htmlTagWorker = await _htmlTagWorkerFinder.FindAsync(id, cancellationToken);
            return _mapper.Map<HtmlTagWorkerDetail>(htmlTagWorker);
        }

        public async Task<Guid> AddHtmlTagWorkerAsync(HtmlTagWorkerDetail data,
           CancellationToken cancellationToken = default)
        {
            var htmlTagWorker = _mapper.Map<HtmlTagWorker>(data);
            _htmlTagWorkerRepository.Insert(htmlTagWorker);
            await _saveChangesCommand.SaveAsync(cancellationToken);
            return htmlTagWorker.ID;
        }

        public async Task<Guid> UpdateHtmlTagWorkerAsync(HtmlTagWorkerDetail data,
          CancellationToken cancellationToken = default)
        {
            var item = await _htmlTagWorkerFinder.FindAsync(data.ID, cancellationToken);
            _mapper.Map(data, item);
            await _saveChangesCommand.SaveAsync(cancellationToken);
            return item.ID;
        }

        public async Task DeleteHtmlTagWorkerAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            var entity = await _htmlTagWorkerFinder.FindAsync(id, cancellationToken);
            _htmlTagWorkerRepository.Delete(entity);
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task AddHtmlTagWorkersAsync(HtmlTagWorkerDetail[] data,
           CancellationToken cancellationToken = default)
        {
            var htmlTagWorkers = _mapper.Map<HtmlTagWorker[]>(data);
            foreach (var htmlTagWorker in htmlTagWorkers)
            {
                _htmlTagWorkerRepository.Insert(htmlTagWorker);
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);
            
        }

        [Obsolete("Настройки надо получать через _settingsBLL и через api/systemsettings/mailservicesetting")]
        public async Task<MailServiceSettings> GetMailSettingsValueAsync(CancellationToken cancellationToken = default)
        {
            _cache.Remove("settings");

            var settings = await _settingsBLL.GetValueAsync(SystemSettings.MailServiceSetting, cancellationToken);
            if (settings != null)
            {
                var settingsValue = Encoding.UTF8.GetString(settings);
                var mailSetting = JsonSerializer.Deserialize<MailServiceSettings>(settingsValue);
                return mailSetting;
            }

            return CreateDefault();
        }


        public static MailServiceSettings CreateDefault()
        {
            var mailManagerConfiguration = new MailManagerConfiguration()
            {
                InboundProtocol = ServiceBase.MailService.WebAPIModels.Protocol.POP3,
                POP3UserName = string.Empty,
                POP3UserPasswordEncrypted = string.Empty,
                POP3Server = string.Empty,
                POP3Port = 110,
                InboundUseSsl = true,
                POP3DefaultEncoding = "koi8-r",
                CheckForMail = true,
                CheckInterval = 600000,
                ProcessAttachments = true,
                MaxAttachmentSize = 2097152,
                ReplyToClient = true,
                SMTPUserName = string.Empty,
                SMTPUserPasswordEncrypted = string.Empty,
                SMTPSenderName = string.Empty,
                SMTPSenderEmail = string.Empty,
                SMTPServer = string.Empty,
                SMTPPort = 25,
                SMTPAutenticationRequired = false,
                SMTPTimeout = 5000,
                SMTPDefaultEncoding = "koi8-r",
                UseExceptions = false,
                Policy = Core.PolicyType.Deny,
                IsSSLEnabled = false,
                IsToProcessEmailWithoutTitle = false,
                IsToProcessEmailWithoutContent = false,
                ExceptionElements = null,
                IgnoreSertificates = false,
            };
            var mailServiceSettings = new MailServiceSettings()
            {
                EnableTrace = false,
                MailManagerConfiguration = mailManagerConfiguration,
                Port = 64802,
                WorkflowServiceBaseURL = ApplicationManager.Instance.WorkflowServiceBaseURL
            };
            return mailServiceSettings;
        }



        public async Task SetMailSettingsValueAsync(MailServiceSettings mailSetting, CancellationToken cancellationToken = default)
        {
            var mailSettingString = JsonSerializer.Serialize(mailSetting);
            var settings = Encoding.UTF8.GetBytes(mailSettingString);
            var mailSettingValue = await _settingsFinder.FindAsync(SystemSettings.MailServiceSetting, cancellationToken);
            if (mailSettingValue == null)
            {
                var serviceSettings = new Setting(SystemSettings.MailServiceSetting, settings);
                _settingsRepository.Insert(serviceSettings);
            }
            else
            {
                mailSettingValue.Value = settings;
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        
    }
}
