using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Messages;
using InfraManager.Core;

namespace IM.Core.HttpClient.ServiceDesk;

public class EMailTemplateClient : ClientWithAuthorization
{
    private const string Url = "EMailTemplate";

    public EMailTemplateClient()
        : base(ApplicationManager.Instance.WebAPIBaseURL)
    {
    }

    /// <summary>
    /// Получить подготовленное по шаблону письмо асинхронно. 
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Экземпляр <see cref="EMailTemplateDetails"/>, содержащий данные письма (тема, тело).</returns>
    public async Task<EMailTemplateDetails> GetAsync(EMailTemplateRequest request, CancellationToken cancellationToken = default)
    {
        return await GetAsync<EMailTemplateDetails, EMailTemplateRequest>(Url, request, null, cancellationToken);
    }
}