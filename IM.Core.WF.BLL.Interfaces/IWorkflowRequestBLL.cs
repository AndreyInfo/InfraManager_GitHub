using System;

namespace IM.Core.WF.BLL.Interfaces
{
    public interface IWorkflowRequestBLL
    {
        void Insert(Guid id);

        void Delete(Guid? id);

        bool Exists(Guid id);
    }
}
