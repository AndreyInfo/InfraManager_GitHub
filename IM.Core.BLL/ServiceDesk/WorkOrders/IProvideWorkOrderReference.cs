using InfraManager.DAL.ServiceDesk.WorkOrders;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    /// <summary>
    /// Этот интерфейс ищет существующий объект WorkOrderReference по идентификатору связанного объекта
    /// или создает новый
    /// </summary>
    internal interface IProvideWorkOrderReference
    {
        Task<WorkOrderReference> GetOrCreateAsync(InframanagerObject? referencedObject, CancellationToken cancellationToken = default);
    }
}
