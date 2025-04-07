using InfraManager.BLL.Settings;
using InfraManager.ResourcesArea;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;


namespace InfraManager.BLL.Localization
{
    internal class StaticResourceLocalizer : ILocalizeText, ISelfRegisteredService<ILocalizeText>
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICultureProvider _cultureProvider;

        public StaticResourceLocalizer(
            ICurrentUser currentUser,
            ICultureProvider userSettings)
        {
            _currentUser = currentUser;
            _cultureProvider = userSettings;
        }

        public string Localize(string text, string locale)
        {
            return Resources.ResourceManager.GetString(text, new CultureInfo(locale)) ?? text;
        }

        public string Localize(string text)
        {
            return Localize(text, _cultureProvider.GetCurrentCulture());
        }

        public async Task<string> LocalizeAsync(string text, CancellationToken cancellationToken = default)
        {
            var cultureName = await _cultureProvider.GetCurrentCultureAsync(cancellationToken);
            return Localize(text, cultureName);
        }
    }
}
