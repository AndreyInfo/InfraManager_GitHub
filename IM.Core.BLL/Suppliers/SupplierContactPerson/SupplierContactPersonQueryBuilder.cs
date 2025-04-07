using Inframanager.BLL;
using InfraManager.DAL;
using SupplierContactPersonEntity = InfraManager.DAL.Suppliers.SupplierContactPerson;

namespace InfraManager.BLL.Suppliers.SupplierContactPerson;

internal sealed class SupplierContactPersonQueryBuilder 
    : IBuildEntityQuery<SupplierContactPersonEntity, SupplierContactPersonDetails, SupplierContactPersonFilter>
    , ISelfRegisteredService<IBuildEntityQuery<SupplierContactPersonEntity, SupplierContactPersonDetails, SupplierContactPersonFilter>>
{
    private readonly IReadonlyRepository<SupplierContactPersonEntity> _repository;

    public SupplierContactPersonQueryBuilder(IReadonlyRepository<SupplierContactPersonEntity> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<SupplierContactPersonEntity> Query(SupplierContactPersonFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterBy.Name.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filterBy.Surname))
        {
            query = query.Where(x => x.Surname.ToLower().Contains(filterBy.Surname.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filterBy.Phone))
        {
            query = query.Where(x => x.Phone.Contains(filterBy.Phone));
        }

        if (!string.IsNullOrWhiteSpace(filterBy.Email))
        {
            query = query.Where(x => x.Email.ToLower().Contains(filterBy.Email.ToLower()));
        }

        return query;
    }
}