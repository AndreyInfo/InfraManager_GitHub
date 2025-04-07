using InfraManager.BLL.Settings;
using InfraManager.Web.Helpers;
using Microsoft.AspNetCore.Http;

namespace InfraManager.UI.Web.Helpers
{
    public class ClientCultureProvider : IDefaultClientCultureProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientCultureProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string ClientCultureName => _httpContextAccessor.HttpContext.GetSupportedBrowserCultureName();
    }
}
