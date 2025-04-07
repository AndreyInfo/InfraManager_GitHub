using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;

namespace InfraManager.BLL.Users.Events;

public class UserRoleEventWriterConfigurer : IConfigureEventWriter<UserRole, User>
{
    public void Configure(IEventWriter<UserRole, User> writer)
    {
    }
}