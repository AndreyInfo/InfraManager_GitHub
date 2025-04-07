using Inframanager.BLL.EventsOld;
using InfraManager.DAL.AccessManagement;

namespace InfraManager.BLL.AccessManagement.Roles;

public class RolesConfigureEventBuilder : IConfigureEventBuilder<Role>
{
    public void Configure(IBuildEvent<Role> config)
    {
        config.HasId(x => x.ID);
        config.HasEntityName(nameof(Role));
        config.HasInstanceName(x => nameof(Role));
    }

    public void WhenInserted(IBuildEventOperation<Role> insertConfig)
    {
        insertConfig.HasOperation(OperationID.Roles_Add, role => $"Добавлена [Роль] '{role.Name}");
    }

    public void WhenUpdated(IBuildEventOperation<Role> updateConfig)
    {
        updateConfig.HasOperation(OperationID.Role_Update, role => $"Сохранена [Роль] '{role.Name}");
    }

    public void WhenDeleted(IBuildEventOperation<Role> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.Role_Delete, role => $"Удалена [Роль] '{role.Name}");
    }
}