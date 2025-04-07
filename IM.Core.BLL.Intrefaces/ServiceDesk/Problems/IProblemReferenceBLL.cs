using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.ServiceDesk.Problems;

public interface IProblemReferenceBLL
{
    Task<ProblemListItem[]> GetReferencedProblemsAsync(BaseFilter filter, Guid objectID,
        ObjectClass referenceObjectClass, CancellationToken cancellationToken = default);
}