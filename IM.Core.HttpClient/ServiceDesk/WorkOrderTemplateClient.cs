using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

namespace IM.Core.HttpClient.ServiceDesk;

public class WorkOrderTemplateClient : ClientWithAuthorization
{
    private const string Url = "WorkOrderTemplates/";

    public WorkOrderTemplateClient(string baseUrl) : base(baseUrl)
    { }
    
    public async Task<WorkOrderTemplateDetails> GetAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        return await GetAsync<WorkOrderTemplateDetails>($"{Url}{id}", userId, cancellationToken);
    }
}