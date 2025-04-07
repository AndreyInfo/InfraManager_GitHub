using System;

namespace IM.Core.DM.BLL.Interfaces
{
    public interface INegotiationDataManagerBLL
    {
        bool NegotiationExistsByObject(Guid objectID, Guid userID);
    }
}
