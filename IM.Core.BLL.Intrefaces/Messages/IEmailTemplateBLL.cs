using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Messages
{
    /// <summary>
    /// Формирование писем по шаблонам.
    /// </summary>
    public interface IEmailTemplateBLL
    {
        /// <summary>
        /// Получить подготовленное по шаблону письмо. 
        /// </summary>
        /// <param name="request">Запрос на получение письма.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
        /// <returns>Экземпляр <see cref="EMailTemplateDetails"/>, содержащий данные подготовленного письма.</returns>
        public Task<EMailTemplateDetails> GetEmailTemplateAsync(EMailTemplateRequest request, CancellationToken cancellationToken = default);
    }
}
