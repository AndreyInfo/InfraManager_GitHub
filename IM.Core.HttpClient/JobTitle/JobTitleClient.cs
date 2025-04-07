using InfraManager.BLL.OrganizationStructure;
using InfraManager.BLL.OrganizationStructure.JobTitles;
using InfraManager.BLL.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.JobTitle
{
    public class JobTitleClient : ClientWithAuthorization
    {
        internal static string _url = "JobTitles/";

        public JobTitleClient(string baseUrl) : base(baseUrl)
        {
        }
        public async Task<JobTitleDetails[]> GetListAsync(Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<JobTitleDetails[]>($"{_url}", userId, cancellationToken);
        }
    }
}
