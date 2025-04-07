using IM.Core.DM.BLL.Interfaces.Models;

namespace IM.Core.DM.BLL.Interfaces
{ 
    public interface IOperationDataManagerBLL
    {
        OperationModel[] GetList();
    }
}
