using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software.PackageContents;
internal class SoftwareModelPackageContentsQuery : ISoftwareModelPackageContentsQuery, ISelfRegisteredService<ISoftwareModelPackageContentsQuery>
{
    private readonly DbSet<Manufacturer> _manufacturerDbSet;
    private readonly DbSet<SoftwareInstallation> _softwareInstallationDbSet;

    public SoftwareModelPackageContentsQuery(
        DbSet<Manufacturer> manufacturerDbSet,
        DbSet<SoftwareInstallation> softwareInstallationDbSet
        )
    {
        _manufacturerDbSet = manufacturerDbSet;
        _softwareInstallationDbSet = softwareInstallationDbSet;
    }

    public async Task<SoftwareModelPackageContentsItem[]> ExecuteAsync(PaggingFilter filter, IOrderedQueryable<SoftwareModel> orderedQuery, CancellationToken cancellationToken)
    {
        var query = orderedQuery.Select(x => new SoftwareModelPackageContentsItem
        {
            ID = x.ID,
            Name = x.Name,
            Code = x.Code,
            Version = x.Version,
            Template = x.Template,
            ExternalID = x.ExternalID,
            ModelRedaction = x.ModelRedaction,
            SoftwareTypeName = x.SoftwareType.Name,
            ManufacturerID = (Guid)x.ManufacturerID,
            SoftwareModelUsingTypeName = x.SoftwareModelUsingType.Name,
            InstallationCount = _softwareInstallationDbSet.AsNoTracking().Count(y => y.SoftwareModelID == x.ID),
            LanguageID = x.LicenseModelAdditionFields.LanguageID ?? SoftwareModelLanguage.Undefined,
            ManufacturerName = _manufacturerDbSet.AsNoTracking().FirstOrDefault(y => y.ImObjID == x.ManufacturerID).Name
        });

        if (!string.IsNullOrEmpty(filter.SearchString))
            query = query.Where(c => c.Name.ToLower().Contains(filter.SearchString.ToLower()));
        
        if (filter.Take > 0)
        {
            query = query.Skip(filter.Skip).Take(filter.Take);
        }

        return await query.ToArrayAsync(cancellationToken);
    }
}
