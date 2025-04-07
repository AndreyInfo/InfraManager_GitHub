using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.DataProviders
{
    internal class SoftwareLicenceSchemeDataProvider : ISoftwareLicenceSchemeDataProvider, ISelfRegisteredService<ISoftwareLicenceSchemeDataProvider>
    {
        private readonly CrossPlatformDbContext _softwareLicenceSchemeDataContext;

        public SoftwareLicenceSchemeDataProvider(CrossPlatformDbContext softwareLicenceSchemeDataContext)
        {
            _softwareLicenceSchemeDataContext= softwareLicenceSchemeDataContext?? throw new ArgumentNullException(nameof(softwareLicenceSchemeDataContext));
        }

        public async Task<List<SoftwareLicenceScheme>> GetListAsync(string searchText = null, bool showDeleted = false, CancellationToken cancellationToken = default)
        {
            var query = _softwareLicenceSchemeDataContext.Set<SoftwareLicenceScheme>().AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(x => x.Name.Contains(searchText));
            if (!showDeleted)
                query = query.Where(x => !x.IsDeleted);
                
            var result = await query.ToListAsync(cancellationToken);

            return result;

        }

        public async Task<SoftwareLicenceScheme> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                return null;
            return await _softwareLicenceSchemeDataContext.Set<SoftwareLicenceScheme>().Where(x => x.ID == id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<SoftwareLicenceScheme> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            return await _softwareLicenceSchemeDataContext.Set<SoftwareLicenceScheme>().Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Guid> AddAsync(SoftwareLicenceScheme softwareLicenceScheme, CancellationToken cancellationToken = default)
        {
            _softwareLicenceSchemeDataContext.Add(softwareLicenceScheme);
            await _softwareLicenceSchemeDataContext.SaveChangesAsync();
            return softwareLicenceScheme.ID;
        }

        public async Task<List<SoftwareLicenceSchemeProcessorCoeff>> GetCoefficientListAsync(Guid schemeID, CancellationToken cancellationToken = default)
        {
            if (schemeID == Guid.Empty)
                return null;
            return await _softwareLicenceSchemeDataContext.Set<SoftwareLicenceSchemeProcessorCoeff>().Where(x => x.LicenceSchemeId == schemeID).ToListAsync(cancellationToken);
        }

        public Task AddAsync(SoftwareLicenceSchemeProcessorCoeff softwareLicenceSchemeProcessorCoeff, CancellationToken cancellationToken = default)
        {
            _softwareLicenceSchemeDataContext.Add(softwareLicenceSchemeProcessorCoeff);
            return _softwareLicenceSchemeDataContext.SaveChangesAsync();
        }

        public void Delete(SoftwareLicenceSchemeProcessorCoeff softwareLicenceSchemeProcessorCoeff)
        {
            _softwareLicenceSchemeDataContext.Remove(softwareLicenceSchemeProcessorCoeff);
            _softwareLicenceSchemeDataContext.SaveChanges();
        }
    }
}
