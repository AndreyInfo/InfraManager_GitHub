using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software
{
    /// <summary>
    /// ПОставщик аднных для сущности "Модель ПО"
    /// </summary>
    public interface ISoftwareModelDataProvider
    {
        Task<SoftwareModel> GetAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
