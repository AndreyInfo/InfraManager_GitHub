using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    public interface IProblemTypesDataProvider
    {
        Task<ProblemType> GetRootProblemTypeAsync();

        Task<List<ProblemType>> GetPathInTreeByIdAsync(Guid problemTypeFindPathID);

        Task<List<ProblemType>> GetChildrenByIdAsync(Guid parentId, List<Guid> filterId); 
    }
}
