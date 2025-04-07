using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk
{
    public interface IWorkOrderDataProvider
    {
        Task UpdateToDefultPriorityByPriorityIdAsync(Guid priorityId, Guid defultPriorityId);
    }
}
