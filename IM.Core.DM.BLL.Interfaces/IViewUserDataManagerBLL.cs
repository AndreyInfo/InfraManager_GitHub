using IM.Core.DM.BLL.Interfaces.Models;
using System;

namespace IM.Core.DM.BLL.Interfaces
{
    public interface IViewUserDataManagerBLL
    {
        ViewUserModel Get(int id);
        ViewUserModel Get(Guid id);

        bool ExistsByID(Guid id);

        bool ExistsByEmail(Guid id, string email);
    }
}
