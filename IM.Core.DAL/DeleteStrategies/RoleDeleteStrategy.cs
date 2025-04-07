using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.AccessManagement;

namespace InfraManager.DAL.DeleteStrategies;

internal class RoleDeleteStrategy : IDeleteStrategy<Role>,
        ISelfRegisteredService<IDeleteStrategy<Role>>
{
    private readonly IRepository<RoleOperation> _roleOperations;
    private readonly IRepository<RoleLifeCycleStateOperation> _roleLifeCycleStateOperations;
    private readonly DbSet<Role> _roles;

    public RoleDeleteStrategy(IRepository<RoleOperation> roleOperations,
        IRepository<RoleLifeCycleStateOperation> roleLifeCycleStateOperations,
        DbSet<Role> roles)
    {
        _roleOperations = roleOperations;
        _roleLifeCycleStateOperations = roleLifeCycleStateOperations;
        _roles = roles;
    }

    public void Delete(Role entity)
    {
        entity.Operations.ForEach(c => _roleOperations.Delete(c));
        entity.LifeCycleStateOperations.ForEach(c => _roleLifeCycleStateOperations.Delete(c));
        _roles.Remove(entity);
    }
}
