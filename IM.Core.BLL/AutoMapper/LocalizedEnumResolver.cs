using AutoMapper;
using InfraManager.BLL.Localization;
using System;

namespace InfraManager.BLL.AutoMapper
{
    public class LocalizedEnumResolver<TSrc, TDest, TEnum> :
        IMemberValueResolver<TSrc, TDest, TEnum, string>
        where TEnum : struct, Enum
    {
        private readonly ILocalizeEnum<TEnum> _localizer;

        public LocalizedEnumResolver(ILocalizeEnum<TEnum> localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(TSrc source, TDest destination, TEnum sourceMember, string destMember, ResolutionContext context)
        {
            return _localizer.Localize(sourceMember);
        }
    }
}
