using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Messages
{
    /// <summary>
    /// Формирование шаблонов писем
    /// </summary>
    public interface INotificationTemplateBLL
    {
        /// <summary>
        /// Получить подготовленное по шаблону письмо.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
        /// <returns>Модель шаблона письма/рассылки</returns>
        public Task<EMailTemplateDetails> GetEMailTemplateAsync(EMailTemplateRequest request, CancellationToken cancellationToken);
    }
}
