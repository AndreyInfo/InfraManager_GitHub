using InfraManager.BLL.Asset;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.OrganizationStructure
{
    public class GroupClient : ClientWithAuthorization
    {
        private const string Url = "group/queue";

        public GroupClient(string baseUrl) : base(baseUrl) { }

        public Task<GroupDetails> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
            GetAsync<GroupDetails>($"{Url}/{id}", cancellationToken: cancellationToken);

        public Task<GroupDetails[]> GetExecutorListAsync(GroupFilter filter, CancellationToken cancellationToken = default)
        {
            var request = new
            {
                SDExecutor = filter.SDExecutor,
                HasAccessToObjectID = filter.HasAccessToObjectID,
                HasAccessToObjectClassID = filter.HasAccessToObjectClassID,
                TTZEnabled = filter.TTZEnabled,
                TOZEnabled = filter.TOZEnabled,
                ServiceResponsibilityEnabled = filter.ServiceResponsibilityEnabled,
                QueueIDList = filter.QueueIDList,
            };
            return GetAsync<GroupDetails[], object>($"{Url}/table", request, null, cancellationToken);
        }
    }
}
