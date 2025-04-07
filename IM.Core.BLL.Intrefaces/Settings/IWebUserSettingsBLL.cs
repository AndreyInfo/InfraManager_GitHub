using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    public interface IWebUserSettingsBLL
    {        
        ValueTask<WebUserSettings> GetAsync(Guid userId, CancellationToken cancellationToken = default);
        Task SetAsync(Guid userId, WebUserSettings settings, CancellationToken cancellationToken = default);
        Task ResetAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
