using AutoMapper;
using InfraManager.BLL.Localization;
using InfraManager.BLL.Settings;
using System;

namespace InfraManager.BLL.AutoMapper
{
    public class ManhoursResolver<TSource, TDestination> :
        IMemberValueResolver<TSource, TDestination, int, string>
    {
        private const string FormatText = "ManhoursDisplayFormat";

        private readonly ICultureProvider _cultureProvider;
        private readonly ILocalizeText _localizer;

        public ManhoursResolver(
            ICultureProvider cultureProvider,
            ILocalizeText localizer)
        {
            _cultureProvider = cultureProvider;
            _localizer = localizer;
        }

        public string Resolve(TSource source, TDestination destination, int sourceMember, string destMember, ResolutionContext context)
        {
            var cultureName = _cultureProvider.GetCurrentCulture();

            return _localizer.ManhoursToString(cultureName, sourceMember);
        }
    }
}
