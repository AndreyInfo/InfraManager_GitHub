using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software
{
    internal class SoftwateModelDataProvider : ISoftwareModelDataProvider
    {
        private readonly IQueryable<SoftwareModel> _softwareDataContext;
        public SoftwateModelDataProvider(DbSet<SoftwareModel> softwareDataContext)
        {
            _softwareDataContext = softwareDataContext
                .Include(x => x.SoftwareType)
                .Include(x => x.CommercialModel)
                .Include(x => x.Parent)
                .Include(x => x.True)
                .Include(x => x.SoftwareModelUsingType);
        }
        public async Task<SoftwareModel> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _softwareDataContext.Where(x => x.ID == id).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
