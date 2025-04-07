using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL;
using InfraManager.DAL.Database.Import;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.BLL.Database.Import;

public class UIDBFieldsFinderQueryBuilder:IFinderQuery<Guid,UIDBFields>
{
    private readonly IReadonlyRepository<UIDBFields> _repository;

    public UIDBFieldsFinderQueryBuilder(IReadonlyRepository<UIDBFields> repository)
    {
        _repository = repository;
    }

    public async Task<UIDBFields> GetFindQueryAsync(Guid key, CancellationToken token)
    {
        return await _repository.With(x => x.Configuration).Query()
            .SingleOrDefaultAsync(x => x.ID == key, token);
    }
}