using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Software;
using InfraManager.DAL.Software.Installation;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.DataProviders;

internal class SoftwareInstallationDataProvider : ISoftwareInstallationDataProvider,
    ISelfRegisteredService<ISoftwareInstallationDataProvider>
{
    private readonly CrossPlatformDbContext _infraManagerDataContext;

    public SoftwareInstallationDataProvider(CrossPlatformDbContext infraManagerDataContext)
    {
        _infraManagerDataContext =
            infraManagerDataContext ?? throw new ArgumentNullException(nameof(infraManagerDataContext));
    }

    public void Add(SoftwareInstallation softwareInstallation)
    {
        _infraManagerDataContext.Add(softwareInstallation);
    }

    public void Remove(SoftwareInstallation softwareInstallation)
    {
        _infraManagerDataContext.Remove(softwareInstallation);
    }

    public async Task<IQueryable<ViewSoftwareInstallation>> GetViewListAsync(CancellationToken cancellationToken)
        => _infraManagerDataContext.Set<ViewSoftwareInstallation>();

    public async Task<SoftwareInstallation> GetAsync(Guid ID, CancellationToken cancellationToken)
    {
        return await _infraManagerDataContext.Set<SoftwareInstallation>()
            .Include(SoftwareInstallation => SoftwareInstallation.SoftwareModel)
            .ThenInclude(sm => sm.CommercialModel)
            .Where(SoftwareInstallation => SoftwareInstallation.ID == ID).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<SoftwareInstallation> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _infraManagerDataContext.Set<SoftwareInstallation>()
            .Where(SoftwareInstallation => SoftwareInstallation.UniqueNumber == name)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<SoftwareInstallationDependances>> GetDependancesAsync(Guid installationID,
        CancellationToken cancellationToken)
    {
        return await _infraManagerDataContext.Set<SoftwareInstallationDependances>()
            .Where(sid => sid.InstallationId == installationID).ToListAsync(cancellationToken);
    }

    public void AddDependant(SoftwareInstallationDependances dependances)
    {
        _infraManagerDataContext.Add(dependances);
    }

    public void RemoveDependant(SoftwareInstallationDependances dependances)
    {
        _infraManagerDataContext.Remove(dependances);
    }
}