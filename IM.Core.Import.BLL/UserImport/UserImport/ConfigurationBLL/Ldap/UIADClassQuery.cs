using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import;

public class UIADClassQuery : IFilterEntity<UIADClass, UIADClassFilter>,
    ISelfRegisteredService<IFilterEntity<UIADClass, UIADClassFilter>>
{
    private readonly IReadonlyRepository<UIADClass> _repository;

    public UIADClassQuery(IReadonlyRepository<UIADClass> repository)
    {
        _repository = repository;
    }

    public IQueryable<UIADClass> Query(UIADClassFilter filter)
    {
        var query = _repository
            .Query();

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));


        return query;
    }
}