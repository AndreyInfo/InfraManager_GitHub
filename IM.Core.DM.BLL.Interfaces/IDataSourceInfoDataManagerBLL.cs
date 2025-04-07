using System;

namespace IM.Core.DM.BLL.Interfaces
{
    public interface IDataSourceInfoDataManagerBLL
    {
        int CheckDataSource(Guid dbID, Guid ownerID);

        Guid RegisterOwner(Guid ownerID, string processName, string machineName, string ipAddresses);

        Tuple<string, string, string> GetOwnerInfo(Guid ownerID);
    }
}
