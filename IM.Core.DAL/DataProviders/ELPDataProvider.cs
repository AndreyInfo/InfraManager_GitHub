using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.DataProviders;

internal class ELPDataProvider : IELPSettingDataProvider, ISelfRegisteredService<IELPSettingDataProvider>
{
    private readonly DbContext _infraManagerDataContext;

    public ELPDataProvider(CrossPlatformDbContext infraManagerDataContext)
    {
        _infraManagerDataContext =
            infraManagerDataContext ?? throw new ArgumentNullException(nameof(infraManagerDataContext));
    }

    public void Add(ElpSetting elpsetting)
    {
        _infraManagerDataContext.Add(elpsetting);
    }

    public void Remove(ElpSetting elpsetting)
    {
        _infraManagerDataContext.Remove(elpsetting);
    }

    public async Task<ElpSetting> GetAsync(Guid ID, CancellationToken cancellationToken)
    {
        return await _infraManagerDataContext.Set<ElpSetting>().Include(elp => elp.Vendor).Where(elp => elp.Id == ID)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ElpSetting> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _infraManagerDataContext.Set<ElpSetting>().Where(elp => elp.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<ElpSetting>> GetListAsync(string searchText, CancellationToken cancellationToken)
    {
        IQueryable<ElpSetting> query = _infraManagerDataContext.Set<ElpSetting>().Include(elp => elp.Vendor);
        if (!string.IsNullOrWhiteSpace(searchText))
            query = query.Where(elp => elp.Name.Contains(searchText));

        return await query.ToListAsync();
    }
}