using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Messages;

internal class EmailTemplateBLL : IEmailTemplateBLL, ISelfRegisteredService<IEmailTemplateBLL>
{
    private readonly IServiceMapper<ObjectClass, INotificationTemplateBLL> _templateMapper;

    public EmailTemplateBLL(IServiceMapper<ObjectClass, INotificationTemplateBLL> templateMapper)
    {
        _templateMapper = templateMapper;
    }

    public async Task<EMailTemplateDetails> GetEmailTemplateAsync(EMailTemplateRequest request, CancellationToken cancellationToken = default)
    {
        var templateBll = _templateMapper.Map(request.ClassID);
        return await templateBll.GetEMailTemplateAsync(request, cancellationToken);
    }
}