using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.ChangeRequests;

namespace IM.Core.HttpClient.ServiceDesk;

public class ChangeRequestTypeClient : ClientWithAuthorization
{
    private const string Url = "RfcTypes";

    public ChangeRequestTypeClient(string baseUrl) : base(baseUrl)
    {
    }
    
    public async Task<RfcTypeDetailsModel> GetAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        return await GetAsync<RfcTypeDetailsModel>($"{Url}/{id}", userId, cancellationToken);
    }
}