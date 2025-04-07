using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.OrganizationStructure
{
    internal class OwnerDataProvider : IOwnerDataProvider, ISelfRegisteredService<IOwnerDataProvider>
    {
        private readonly CrossPlatformDbContext _context;

        public OwnerDataProvider(CrossPlatformDbContext context)
        {
            _context = context;
        }

        public async Task<Owner> GetOwnerAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Owner>().FirstAsync(cancellationToken);
        }
    }
}
