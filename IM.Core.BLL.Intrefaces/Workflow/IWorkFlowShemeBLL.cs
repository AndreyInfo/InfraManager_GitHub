using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Workflow
{
    public interface IWorkFlowShemeBLL
    {
        Task<WorkflowSchemeDetailsModel> FindByIdentifierAsync(string identifier, CancellationToken cancellationToken = default);
        WorkflowSchemeTypeModel[] GetAvailableTypes();
    }
}
