using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Calls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class CallSummaryClient : ClientWithAuthorization
    {
        internal static string _url = "CallSummary/";
        public CallSummaryClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<CallSummaryDetails[]> GetCallSummariesAsync(CallSummaryFilter filter, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetListByPostAsync<CallSummaryDetails[],CallSummaryFilter>($"{_url}byFilter", filter, userId, cancellationToken);
        }
        public async Task<Guid> SaveOrUpdateCallSummaryAsync(CallSummaryDetails data, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await PostAsync<Guid,CallSummaryDetails>($"{_url}saveOrUpdate", data, userId, cancellationToken);
        }
    }
}
