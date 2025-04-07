using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.SoftwareType
{
    public interface ISoftwareTypeBLL
    {
        Task<InfraManager.DAL.Software.SoftwareType> GetSoftwareType(Guid softwareTypeID, CancellationToken cancellationToken);
        Task<bool> SaveAsync(InfraManager.DAL.Software.SoftwareType model, CancellationToken cancellationToken);
        Task RemoveAsync(InfraManager.DAL.Software.SoftwareType model, CancellationToken cancellationToken);
    }
}
