using System.Threading.Tasks;
using System.Threading;
using System;

namespace InfraManager.BLL.ServiceCatalogue
{
    public interface ISlaApplicabilityBLL
    {
        Task<Guid[]> AttendanceItemsAsync(Guid? userId, ObjectClass objectClass, int skip, int take, CancellationToken cancellationToken);
    }
}
