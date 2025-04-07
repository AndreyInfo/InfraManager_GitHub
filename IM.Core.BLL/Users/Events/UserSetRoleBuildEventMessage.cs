using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;

namespace InfraManager.BLL.Users.Events;

public class UserSetRoleBuildEventMessage : IBuildEventMessage<UserRole, User>
{
    private IFinder<UserRole> _finder;

    public UserSetRoleBuildEventMessage(IFinder<UserRole> finder)
    {
        _finder = finder;
    }

    public string Build(UserRole entity, User subject)
    {
        var role = _finder.With(x => x.Role).Find(entity.RoleID, entity.UserID);

        return $"Назначена роль '{role.Role.Name}'";
    }
}

public class UserUnSetRoleBuildEventMessage : IBuildEventMessage<UserRole, User>
{
    private IFinder<Role> _finder;

    public UserUnSetRoleBuildEventMessage(IFinder<Role> finder)
    {
        _finder = finder;
    }

    public string Build(UserRole entity, User subject)
    {
        var role = _finder.Find(entity.RoleID);

        return $"Убрана роль '{role.Name}'";
    }
}