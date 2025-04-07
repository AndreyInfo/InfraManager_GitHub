using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk.ChangeRequests;

namespace IM.Core.HttpClient.ServiceDesk;

public class RfcResultsClient : ClientWithAuthorization
{
    private const string Url = "rfcresults/";
    
    public RfcResultsClient(string baseUrl) : base(baseUrl)
    {
    }
    
    public async Task<ChangeRequestResultDetailsModel> GetAsync(Guid guid, 
        Guid? userId = null,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<ChangeRequestResultDetailsModel>($"{Url}{guid}", userId, cancellationToken);
    }
    
    public async Task<ChangeRequestResultListItem[]> GetList(ListFilter listFilter = null, 
        Guid? userId = null,
        CancellationToken cancellationToken = default)
    {
        return await GetListAsync<ChangeRequestResultListItem[]>(Url, listFilter, userId, cancellationToken);
    }
}