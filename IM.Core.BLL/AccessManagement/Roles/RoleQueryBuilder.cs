using Inframanager.BLL;
using InfraManager.BLL.Roles;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;

namespace InfraManager.BLL.AccessManagement.Roles;

public class RoleQueryBuilder:
    IBuildEntityQuery<Role, RoleDetails, RoleFilter>,
    ISelfRegisteredService<IBuildEntityQuery<Role, RoleDetails, RoleFilter>>
{
    private readonly IReadonlyRepository<Role> _repository;
    
    public RoleQueryBuilder(IReadonlyRepository<Role> repository)
    {
        _repository = repository;
    }
    public IExecutableQuery<Role> Query(RoleFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.Name))
        {
            query = query.Where(role => role.Name.ToLower() == filterBy.Name.ToLower());
        }
        
        return query;
    }
}