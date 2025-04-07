using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.BLL.ServiceDesk.DTOs;
using System.Threading;

namespace InfraManager.BLL.ServiceDesk
{
    public interface IPriorityMatrixBLL
    {
        public Task<ConcordanceDetails[]> GetTableAsync(CancellationToken cancellationToken = default);

        public Task<bool> SaveCellAsync(ConcordanceDetails cell, CancellationToken cancellationToken = default);

        public Task<bool> RemoveCellAsync(Guid urgencyId, Guid influencyId,
            CancellationToken cancellationToken = default);
    }
}

