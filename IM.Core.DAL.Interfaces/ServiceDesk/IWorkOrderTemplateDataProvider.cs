using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InfraManager.DAL.ServiceDesk
{
    public interface IWorkOrderTemplateDataProvider //TODO Refactor this
    {
        Task<List<WorkOrderTemplateFolder>> GetPathFolders(Guid id);
    }
}
