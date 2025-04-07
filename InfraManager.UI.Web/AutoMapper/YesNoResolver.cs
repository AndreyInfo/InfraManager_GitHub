using AutoMapper;
using InfraManager.BLL.Localization;
using InfraManager.BLL.Settings;

namespace InfraManager.UI.Web.AutoMapper
{
    public class YesNoResolver<TSrc, TDest> : IMemberValueResolver<TSrc, TDest, bool, string>
    {
        private const string Yes = "True";
        private const string No = "False";

        private readonly ICultureProvider _cultureProvider;
        private readonly ILocalizeText _localizer;

        public YesNoResolver(ICultureProvider cultureProvider, ILocalizeText localizer)
        {
            _cultureProvider = cultureProvider;
            _localizer = localizer;
        }

        public string Resolve(TSrc source, TDest destination, bool sourceMember, string destMember, ResolutionContext context)
        {
            return _localizer.Localize(
                sourceMember ? Yes : No,
                _cultureProvider.GetCurrentCulture());
        }
    }
}
