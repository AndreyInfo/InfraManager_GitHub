using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Location;

public interface ILocationNodesQuery
{
    /// <summary>
    /// Получение дочерних узлов в дереве местоположений
    /// </summary>
    /// <param name="parentID">идентиикатор родительнского элемента</param>
    /// <param name="childClassID">тип дочерних узлов, т.к. </param>
    /// <param name="cancellationToken"></param>
    /// <returns>дочерние узлы</returns>
    Task<LocationTreeNode[]> GetNodesAsync(int parentID
        , ObjectClass? childClassID = null
        , CancellationToken cancellationToken = default);
}

