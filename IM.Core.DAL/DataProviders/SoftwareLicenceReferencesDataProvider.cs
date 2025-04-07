using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.DataProviders
{
    internal class SoftwareLicenceReferencesDataProvider : ISoftwareLicenceReferencesDataProvider, ISelfRegisteredService<ISoftwareLicenceReferencesDataProvider>
    {
        private readonly CrossPlatformDbContext _softwareDataContext;
        public SoftwareLicenceReferencesDataProvider(CrossPlatformDbContext softwareDataContext)
        {
            _softwareDataContext = softwareDataContext ?? throw new ArgumentNullException(nameof(softwareDataContext));
        }
        public async Task<IList<SoftwareLicenceReference>> GetListForObjectAsync(int objectClassID, Guid objectID, CancellationToken cancellationToken)
        {
            return await _softwareDataContext.Set<SoftwareLicenceReference>()
                .Include(x=>x.SoftwareLicence)
                .Where(x=>x.ClassId == objectClassID && x.ObjectId == objectID).ToListAsync(cancellationToken);
        }
    }
}
