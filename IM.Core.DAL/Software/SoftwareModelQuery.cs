using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software;
internal class SoftwareModelQuery : ISoftwareModelQuery, ISelfRegisteredService<ISoftwareModelQuery>
{
    private readonly DbSet<Manufacturer> _manufacturerDbSet;

    public SoftwareModelQuery(
        DbSet<Manufacturer> manufacturerDbSet
        )
    {
        _manufacturerDbSet = manufacturerDbSet;
    }

    public async Task<SoftwareModelDetailsItem[]> ExecuteAsync(PaggingFilter filter, IOrderedQueryable<SoftwareModel> orderedQuery, CancellationToken cancellationToken)
    {
        var query = orderedQuery.Select(x => new SoftwareModelDetailsItem
        {
            ID = x.ID,
            Name = x.Name,
            TemplateID = x.Template,
            Version = x.Version,
            SoftwareTypeID = x.SoftwareTypeID,
            Note = x.Note,
            Code = x.Code,
            ExternalID = x.ExternalID,
            SupportDate = x.SupportDate,
            CreateDate = x.CreateDate,
            SoftwareModelUsingTypeName = x.SoftwareModelUsingType.Name,
            IsCommercial = x.Template == SoftwareModelTemplate.CommercialModel,
            ProcessNames = x.ProcessNames,
            ModelRedaction = x.ModelRedaction,
            ModelDistribution = x.ModelDistribution,

            ManufacturerID = x.ManufacturerID 
                ?? _manufacturerDbSet.AsNoTracking().FirstOrDefault(x => x.ID == Manufacturer.EmptyID).ImObjID.Value
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
