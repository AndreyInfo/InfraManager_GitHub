using System;
using System.Threading.Tasks;

namespace IM.Core.DM.BLL.Interfaces
{
    public interface IAssetServiceDataManagerBLL
    {
        Task<string> GetUtilizerFullNameAsync(Guid objectID);
    }
}
