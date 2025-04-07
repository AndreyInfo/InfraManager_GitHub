using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL
{
    public interface IClassIconBLL
    {
        public Task<ClassIcon> GetByIdAsync(ObjectClass classID, CancellationToken cancellationToken = default);
        public Task<string> GetIconNameByClassIDAsync(ObjectClass classID, CancellationToken cancellationToken = default);
        public string GetIconNameByClassID(ObjectClass classID);
        public Task<bool> AddAsync(ClassIcon[] models, CancellationToken cancellationToken = default);
        public Task<bool> DeleteAsync(ClassIcon[] models, CancellationToken cancellationToken = default);
    }
}
