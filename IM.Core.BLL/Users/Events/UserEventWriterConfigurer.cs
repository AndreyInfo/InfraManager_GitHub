using Inframanager.BLL.Events;
using InfraManager.DAL;

namespace InfraManager.BLL.Users.Events;

public class UserEventWriterConfigurer : IConfigureEventWriter<User, User>
{
    public void Configure(IEventWriter<User, User> writer)
    {
    }
}