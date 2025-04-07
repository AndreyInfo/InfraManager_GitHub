using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset
{
    public interface IMeduimBLL
    {
        Task<MediumDetails[]> GetListAsync(CancellationToken cancellationToken);
    }
}
