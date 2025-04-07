using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.OrganizationStructure
{
    public interface IOrganizationStructureBLL
    {
        /// <summary>
        /// Получение следующих узлов по запросу 
        /// </summary>
        /// <param name="nodeRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<OrganizationStructureNodeModelDetails[]> GetNodesAsync(
            OrganizationStructureNodeRequestModelDetails nodeRequest,
            CancellationToken cancellationToken = default);
        
        
        /// <summary>
        /// получение всех родительских узлов, до конкретного
        /// </summary>
        /// <param name="nodeRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        Task<OrganizationStructureNodeModelDetails[]> GetPathToNodeAsync(OrganizationStructureNodeRequestModelDetails nodeRequest, CancellationToken cancellationToken);
    }
}
