using IM.Core.DM.BLL.Interfaces.Models;
using System;
using System.Collections.Generic;

namespace IM.Core.DM.BLL.Interfaces
{
    public interface IUserDataManagerBLL
    {
        HashSet<int> GetGrantedOperations(Guid userID);

        UserModel GetUser(Guid id);
    }
}
