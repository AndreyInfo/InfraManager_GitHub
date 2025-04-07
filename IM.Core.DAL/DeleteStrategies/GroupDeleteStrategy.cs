using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.DAL.DeleteStrategies;

internal class GroupDeleteStrategy : IDeleteStrategy<Group>, ISelfRegisteredService<IDeleteStrategy<Group>>
{
    private readonly IRepository<GroupUser> _groupUserRepository;
    private readonly DbSet<Group> _groups;

    public GroupDeleteStrategy(IRepository<GroupUser> groupUserRepository, DbSet<Group> groups)
    {
        _groupUserRepository = groupUserRepository;
        _groups = groups;
    }

    public void Delete(Group entity)
    {
        entity.QueueUsers.ForEach(c => _groupUserRepository.Delete(c));
        _groups.Remove(entity);
    }
}
