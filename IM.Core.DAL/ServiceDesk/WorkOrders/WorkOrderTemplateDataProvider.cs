using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using InfraManager;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    [Obsolete]
    internal class WorkOrderTemplateDataProvider : IWorkOrderTemplateDataProvider, ISelfRegisteredService<IWorkOrderTemplateDataProvider>
    {
        private readonly DbContext _db;

        public WorkOrderTemplateDataProvider(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<List<WorkOrderTemplateFolder>> GetPathFolders(Guid id)
        {
            var dbSet = _db.Set<WorkOrderTemplateFolder>();

            var query = dbSet.Where(c => c.ID == id)
                             .Include(c => c.Parent);

            var folder = await query.FirstOrDefaultAsync();

            return ConvertTreeToList(folder);
        }



        private List<WorkOrderTemplateFolder> ConvertTreeToList(WorkOrderTemplateFolder folder)
        {
            var result = new List<WorkOrderTemplateFolder>();
            while (folder is not null)
            {
                result.Add(folder);
                folder = folder.Parent;
            }

            return result;
        }
    }
}
