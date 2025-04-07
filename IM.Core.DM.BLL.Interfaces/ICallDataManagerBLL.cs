using IM.Core.DM.BLL.Interfaces.Models;
using System;

namespace IM.Core.DM.BLL.Interfaces
{
    public interface ICallDataManagerBLL
    {
        bool ExistsByID(Guid id, bool? removed);

        CallModel Get(Guid id, Guid? currentUserID);

        bool Update(Guid id, string entityStateID, string entityStateName, Guid? workflowSchemeID, string workflowSchemeIdentifier, string workflowSchemeVersion, bool isRepeatableRead);
    }
}
