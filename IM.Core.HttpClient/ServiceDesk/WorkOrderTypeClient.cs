using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.WorkOrders;

namespace IM.Core.HttpClient.ServiceDesk;

public class WorkOrderTypeClient: ClientWithAuthorization
{
    private readonly string _url = "workOrderTypes/";
    
    public WorkOrderTypeClient(string baseUrl) : base(baseUrl)
    {
    }
    
    public async Task<WorkOrderTypeDetails> GetAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        return await GetAsync<WorkOrderTypeDetails>($"{_url}{id}", userId, cancellationToken);
    }
}