using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.WorkOrders.Priorities;

namespace IM.Core.HttpClient.ServiceDesk;

public class WorkOrderPriorityClient : ClientWithAuthorization
{
    private readonly string _url = "workOrderPriority/";
    
    public WorkOrderPriorityClient(string baseUrl) : base(baseUrl)
    {
    }
    
    public async Task<WorkOrderPriorityDetails> GetAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        return await GetAsync<WorkOrderPriorityDetails>($"{_url}{id}", userId, cancellationToken);
    }
}