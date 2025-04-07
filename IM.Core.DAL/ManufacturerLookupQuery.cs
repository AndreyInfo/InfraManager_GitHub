using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL;

internal class ManufacturerLookupQuery : ILookupQuery
{
    private readonly DbSet<Manufacturer> _manufacturers;

    public ManufacturerLookupQuery(DbSet<Manufacturer> manufacturers)
    {
        _manufacturers = manufacturers;
    }

    public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return Array.ConvertAll(
            await _manufacturers.AsNoTracking()
                .Select(manufacturer => new
                {
                    ID = manufacturer.ID,
                    ManufacturerName = manufacturer.Name,
                }).ToArrayAsync(cancellationToken),
            item => new ValueData
            {
                ID = item.ID.ToString(),
                Info = item.ManufacturerName,
            });
    }
}