using System.Collections.Generic;

namespace InfraManager.DAL.Users;

public interface IAllUserQuery
{
    IEnumerable<User> ExecuteQuery();
}