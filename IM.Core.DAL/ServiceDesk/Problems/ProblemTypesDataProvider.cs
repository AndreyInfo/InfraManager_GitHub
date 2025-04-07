using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    internal class ProblemTypesDataProvider : IProblemTypesDataProvider, ISelfRegisteredService<IProblemTypesDataProvider>
    {
        private readonly CrossPlatformDbContext _db;

        public ProblemTypesDataProvider(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<List<ProblemType>> GetPathInTreeByIdAsync(Guid problemTypeFindPathID)
        {
            var dbSet = _db.Set<ProblemType>();

            var result = new List<ProblemType>();
            while (true)
            {
                var itemPath = await dbSet.Where(c => !c.Removed)
                                          .Include(c=> c.Parent)
                                          .Include(c => c.WorkflowScheme)
                                          .FirstOrDefaultAsync(c=> c.ID == problemTypeFindPathID);

                if (itemPath == null)
                {
                    return result;
                }
                
                result.Add(itemPath);

                if (itemPath.Parent is null)
                    break;

                problemTypeFindPathID = itemPath.Parent.ID;
            }

            result.Reverse();// чтобы список был только от родителя к поисковому
            return result;
        }

        public async Task<List<ProblemType>> GetChildrenByIdAsync(Guid parentId, List<Guid> filterId)
        {
            var dbSet = _db.Set<ProblemType>();
            var dbSetWorkFlow = _db.Set<WorkFlowScheme>();

            #region Выгрузка WorkFlowSheme
            var problemTypeWorkFlowShemes = await dbSet.Where(c => c.ParentProblemTypeID == parentId && !filterId.Contains(c.ID) && !c.Removed)
                                                .Select(c => c.WorkflowSchemeIdentifier)
                                                .Distinct()
                                                .ToArrayAsync();

            var collectIdentifier = await dbSetWorkFlow.Where(c => problemTypeWorkFlowShemes.Contains(c.Identifier))
                                                       .Select(c => c.Identifier)
                                                       .Distinct()
                                                       .ToListAsync();

            var list = new List<WorkFlowScheme>();
            foreach (var indentifier in collectIdentifier)
            {
                var maxVersion = await dbSetWorkFlow.Where(c => c.Identifier == indentifier)
                                                    .MaxAsync(c => c.MinorVersion + c.MajorVersion);

                var actualWorkFlowSheme = await dbSetWorkFlow.FirstOrDefaultAsync(c => c.Identifier == indentifier
                                                                                       && c.MinorVersion + c.MajorVersion == maxVersion
                                                                                       );
                list.Add(actualWorkFlowSheme);
            }
            #endregion

            var result = await dbSet.Where(c => c.ParentProblemTypeID == parentId && !filterId.Contains(c.ID) && !c.Removed)
                                    .ToListAsync();

            result.ForEach(c => c.WorkflowScheme = list.FirstOrDefault(v => v.Identifier == c.WorkflowSchemeIdentifier));
            return result;
        }

        public async Task<ProblemType> GetRootProblemTypeAsync()
        {
            var dbSet = _db.Set<ProblemType>();
            return await dbSet.Include(c => c.WorkflowScheme)
                              .FirstOrDefaultAsync(c => c.Parent == null);
        }
    }
}
