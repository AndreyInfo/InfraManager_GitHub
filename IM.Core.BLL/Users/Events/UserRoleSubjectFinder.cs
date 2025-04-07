using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ChangeTracking;

namespace InfraManager.BLL.Users.Events;

public class UserRoleSubjectFinder : ISubjectFinder<UserRole, User>
{
    private readonly IFindEntityByGlobalIdentifier<User> _find;

    public UserRoleSubjectFinder(IFindEntityByGlobalIdentifier<User> find)
    {
        _find = find;
    }
    
    public User Find(UserRole entity, IEntityState originalState)
    {
        return _find.Find(entity.UserID);
    }
}