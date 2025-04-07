using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    internal static class CultureProviderExtensions
    {
        public static async Task<CultureInfo> GetUiCultureInfoAsync(
            this ICultureProvider service, 
            CancellationToken cancellationToken = default)
        {
            var culture = await service.GetCurrentCultureAsync(cancellationToken);

            return new CultureInfo(culture);
        }
    }
}
