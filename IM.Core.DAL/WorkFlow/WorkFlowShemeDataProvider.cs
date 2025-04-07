using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.WorkFlow
{
    internal class WorkFlowShemeDataProvider : IWorkFlowShemeDataProvider, ISelfRegisteredService<IWorkFlowShemeDataProvider>
    {
        private readonly CrossPlatformDbContext _db;

        public WorkFlowShemeDataProvider(CrossPlatformDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<WorkFlowScheme> GetActualVersionByIdentifierAsync(string identifier, CancellationToken cancellationToken)
        {
            var dbSet = _db.Set<WorkFlowScheme>();
            var result = await dbSet.Where(c => c.Identifier.Equals(identifier))
                                    .OrderByDescending(c => c.MajorVersion)
                                    .ThenByDescending(c => c.MinorVersion)
                                    .SelectWorkflowScheme()
                                    .FirstOrDefaultAsync(cancellationToken);

            return result;
        }


        public async Task<bool> IsExistByIdentifierAsync(string identifier)
        {
            var dbSet = _db.Set<WorkFlowScheme>();
            return await dbSet.SelectWorkflowScheme().AnyAsync(c => c.Identifier.Equals(identifier));
        }
    }

    public static class Helper
    {
        public static IQueryable<WorkFlowScheme> SelectWorkflowScheme(this IQueryable<WorkFlowScheme> query)
        {
            return query.Select(x => new WorkFlowScheme()
            {
                Id = x.Id,
                Identifier = x.Identifier,
                MajorVersion = x.MajorVersion,
                MinorVersion = x.MinorVersion,
                Name = x.Name,
                RowVersion = x.RowVersion,
                PublisherID = x.PublisherID,
                Note = x.Note,
                ObjectClassID = x.ObjectClassID,
                Status = x.Status,
                TraceIsEnabled = x.TraceIsEnabled,
                ModifierID = x.ModifierID,
                UtcDatePublished = x.UtcDatePublished,
                UtcDateModified = x.UtcDateModified,
                WorkflowSchemeFolderID = x.WorkflowSchemeFolderID,               
            });
        } 
    }
}
