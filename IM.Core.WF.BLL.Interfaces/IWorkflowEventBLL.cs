using IM.Core.WF.BLL.Interfaces.Models;
using System;

namespace IM.Core.WF.BLL.Interfaces
{
    public interface IWorkflowEventBLL
    {
        void Delete(Guid workflowID, bool isRepeatableRead);

        WorkflowEventModel[] GetList(Guid workflowID);

        void Insert(Guid workflowID, WorkflowEventModel workflowEvent);
    }
}
