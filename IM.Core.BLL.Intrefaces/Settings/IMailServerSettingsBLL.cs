using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using InfraManager.ServiceBase.MailService.WebAPIModels;
using InfraManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    public interface IMailServerSettingsBLL
    {
        /// <summary>
        /// Возвращает настройки подключения к почтовому сервису
        /// </summary>
        /// <returns></returns>
        Task<ConnectionSettings> GetConnectionSettingsAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Сохраняет настройки подключения к почтовому сервису
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task SaveConnectionSettingsAsync(ConnectionSettings settings, CancellationToken cancellationToken = default);
        /// <summary>
        /// Возвращает настройки SMTP
        /// </summary>
        /// <returns></returns>
        Task<SendMailSettings> GetSendMailSettingsAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Сохраняет настройки SMTP
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task SaveSendMailSettingsAsync(SendMailSettings settings, CancellationToken cancellationToken = default);
        /// <summary>
        /// Возвращает настройки отправки почты
        /// </summary>
        /// <returns></returns>
        Task<GetMailSettings> GetMailSettingsAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Сохраняет настройки отправки почты
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task SaveGetMailSettingsAsync(GetMailSettings settings, CancellationToken cancellationToken = default);
        /// <summary>
        /// Возвращает возможные значения PolicyType
        /// </summary>
        /// <returns></returns>
        ListItem[] GetPolicyTypeValuesList();
        /// <summary>
        /// Возвращает возможные значения протоколов отправки писем
        /// </summary>
        /// <returns></returns>
        ListItem[] GetProlocolValuesList();
        /// <summary>
        /// Возвращает список исключений
        /// </summary>
        /// <returns></returns>
        Task<PolicySettings> GetPolicyTypeListAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Сохраняет список исключений
        /// </summary>
        /// <returns></returns>
        Task SavePolicyTypeListAsync(PolicySettings settings, CancellationToken cancellationToken = default);
        /// <summary>
        /// Тест отправки почты
        /// </summary>
        /// <returns></returns>
        OperationResult TestSMTPSettings(SMTPSettingsTest settings);
        /// <summary>
        /// Тест получения почты
        /// </summary>
        /// <returns></returns>
        OperationResult TestInboundProtocolSettings(POPSettingsTest settings);
        /// <summary>
        /// Возвращает правила цитирования
        /// </summary>
        /// <returns></returns>
        Task<EmailQuoteTrimmerData[]> GetEmailQuoteTrimmersDataAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Возвращает правило цитирования по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EmailQuoteTrimmerData> GetEmailQuoteTrimmerDataAsync(Guid id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Добавляет правила цитирования
        /// </summary>
        /// <returns></returns>
        Task<Guid> AddEmailQuoteTrimmerAsync(EmailQuoteTrimmerData data,
           CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновляет правила цитирования
        /// </summary>
        /// <returns></returns>
        Task<Guid> UpdateEmailQuoteTrimmerAsync(EmailQuoteTrimmerData data,
           CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет правила цитирования
        /// </summary>
        /// <returns></returns>
        Task DeleteEmailQuoteTrimmerAsync(Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает правила удаления
        /// </summary>
        /// <param name="emailQuoteTrimmerId"></param>
        /// <returns></returns>
        Task<HtmlTagWorkerDetail[]> GetHtmlTagWorkerByQuoteTrimmerIdAsync(Guid emailQuoteTrimmerId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Возвращает правила удаления c пагинацией
        /// </summary>
        /// <param name="filter">Фильтр поиска правил удаления</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        HtmlTagWorkerDetail[] GetHtmlTagWorkerByQuoteTrimmerId(HtmlTagWorkerFilter filter);
        /// <summary>
        /// Добавляет правило удаления
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Guid> AddHtmlTagWorkerAsync(HtmlTagWorkerDetail data,
           CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновляет правило удаления
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Guid> UpdateHtmlTagWorkerAsync(HtmlTagWorkerDetail data,
          CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет правило удаления
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteHtmlTagWorkerAsync(Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает правило удаления
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HtmlTagWorkerDetail> GetHtmlTagWorkerAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Множественное добавление правил удаления
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddHtmlTagWorkersAsync(HtmlTagWorkerDetail[] data,
           CancellationToken cancellationToken = default);
        /// <summary>
        /// Возвращает настройки почтового сервиса
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MailServiceSettings> GetMailSettingsValueAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Сохраняет настройки почтового сервиса
        /// </summary>
        /// <param name="mailSetting">наствойки почтовогго сервиса</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SetMailSettingsValueAsync(MailServiceSettings mailSetting, CancellationToken cancellationToken = default);
    }
}
