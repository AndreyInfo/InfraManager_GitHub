using System;
using System.Threading.Tasks;
using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.Calls.DTO;
using System.Threading;

namespace InfraManager.BLL.Calls
{
    public interface ICallTypeBLL
    {
        public Task<CallTypeDetails[]>
            GetByParentIDAsync(Guid? parentId, CancellationToken cancellationToken = default);

        public Task<string[]> DeleteAsync(DeleteModel<Guid>[] deleteModels,
            CancellationToken cancellationToken = default);

        public Task<CallTypeDetails[]> GetPathItemAsync(Guid? id, CancellationToken cancellationToken = default);
        
        Task<CallTypeDetails[]> GetListAsync(CancellationToken cancellationToken = default);
    }
}
