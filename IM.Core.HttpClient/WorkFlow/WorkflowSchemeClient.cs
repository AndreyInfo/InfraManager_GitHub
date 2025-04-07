using InfraManager.BLL;
using InfraManager.BLL.Workflow;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.ServiceBase.WorkflowService;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class WorkflowSchemeClient : ClientWithAuthorization
    {
        internal static string _url = "WorkflowScheme/";
        public WorkflowSchemeClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<WorkflowSchemeDetailsModel> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<WorkflowSchemeDetailsModel>($"{_url}{guid}", userId, cancellationToken);
        }

        public async Task<WorkflowSchemeDetailsModel> SaveAsync(Guid guid, WorkflowSchemeModel WorkflowScheme, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PutAsync<WorkflowSchemeDetailsModel, WorkflowSchemeModel>($"{_url}{guid}", WorkflowScheme, userId, cancellationToken);
        }

        public async Task<WorkflowSchemeDetailsModel> AddAsync(WorkflowSchemeModel WorkflowScheme, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<WorkflowSchemeDetailsModel, WorkflowSchemeModel>(_url, WorkflowScheme, userId, cancellationToken);
        }
        //  TODO: Раскоментировать и поправить модели по готовности WebAPI
        //public async Task<WorkflowSchemeListItemModel[]> GetList(ListFilter listFilter= null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return await GetListAsync<WorkflowSchemeListItemModel[]>(_url, listFilter, userId, cancellationToken);
        //}
    }
}
