using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Finance;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL;

internal class SupplierLookupQuery : ILookupQuery
{
    private readonly DbSet<Supplier> _suppliers;

    public SupplierLookupQuery(DbSet<Supplier> suppliers)
    {
        _suppliers = suppliers;
    }

    public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return Array.ConvertAll(
            await _suppliers.AsNoTracking()
                .Select(supplier => new
                {
                    ID = supplier.ID,
                    SupplierName = supplier.Name,
                }).ToArrayAsync(cancellationToken),
            item => new ValueData
            {
                ID = item.ID.ToString(),
                Info = item.SupplierName,
            });
    }
}