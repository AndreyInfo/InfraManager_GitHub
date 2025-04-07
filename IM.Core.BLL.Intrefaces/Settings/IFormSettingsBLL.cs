using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    public interface IFormSettingsBLL
    {
        Task<WebUserFormSettingsModel> GetAsync(
            string name, 
            CancellationToken cancellationToken = default);
        Task<WebUserFormSettingsModel> SetAsync(
            string name, 
            WebUserFormSettingsModel model, 
            CancellationToken cancellationToken = default);
    }
}
