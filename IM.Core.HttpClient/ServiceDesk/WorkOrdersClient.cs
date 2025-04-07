using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.WorkOrders;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class WorkOrdersClient : ClientWithAuthorization
    {
        private const string Url = "WorkOrders/";

        public WorkOrdersClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<WorkOrderDetailsModel> GetAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            
            var result = await GetAsync<WorkOrderDetailsModel>($"{Url}{id}", userId, cancellationToken);
            if(result!=null)
                result.NoteList = await GetNotesAsync(id, userId, cancellationToken);
            return result;
        }

        public async Task<WorkOrderDetailsModel> PutAsync(Guid id, WorkOrderDataModel WorkOrder, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await PutAsync<WorkOrderDetailsModel, WorkOrderDataModel>($"{Url}{id}", WorkOrder, userId, cancellationToken);
            if(result != null)
                result.NoteList = await GetNotesAsync(id, userId, cancellationToken);
            return result;
        }

        public async Task<WorkOrderDetailsModel> PatchAsync(Guid id, WorkOrderDataModel WorkOrder, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await PatchAsync<WorkOrderDetailsModel, WorkOrderDataModel>($"{Url}{id}", WorkOrder, userId, cancellationToken);
            if(result != null)
                result.NoteList = await GetNotesAsync(id, userId, cancellationToken);
            return result;
        }

        public async Task<WorkOrderDetailsModel> AddAsync(WorkOrderDataModel WorkOrder, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await PostAsync<WorkOrderDetailsModel, WorkOrderDataModel>(Url, WorkOrder, userId, cancellationToken);
            result.NoteList = await GetNotesAsync(result.ID, userId, cancellationToken);
            return result;
        }
        public async Task<WorkOrderListItem[]> GetListAsync(ListFilter listFilter = null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListAsync<WorkOrderListItem[]>(Url + "reports/allWorkOrders", listFilter, userId, cancellationToken);
        }

        public async Task<WorkOrderDetailsModel[]> GetListAsync(WorkOrderListFilter listFilter= null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<WorkOrderDetailsModel[], WorkOrderListFilter>(Url, listFilter, x => PreProcessRequestHeaders(x, userId), cancellationToken);
        }

        public async Task<NoteListItemModel[]> GetNotesAsync(Guid woID, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<NoteListItemModel[]>(Url + $"{woID}/notes", userId, cancellationToken);
        }

        public async Task<DependencyDetails[]> GetDependenciesAsync(Guid callID, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<DependencyDetails[]>(Url + $"{callID}/dependencies", userId, cancellationToken);
        }
    }
}
