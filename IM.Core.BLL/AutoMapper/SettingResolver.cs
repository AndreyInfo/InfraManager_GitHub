using AutoMapper;
using InfraManager.BLL.Settings;

namespace InfraManager.BLL.AutoMapper
{
    internal class SettingResolver<TSrc, TDest, T> :
        IMemberValueResolver<TSrc, TDest, SystemSettings, T>
    {
        private readonly ISettingsBLL _settings;
        private readonly IConvertSettingValue<T> _converter;

        public SettingResolver(ISettingsBLL settings, IConvertSettingValue<T> converter)
        {
            _settings = settings;
            _converter = converter;
        }

        public T Resolve(TSrc source, TDest destination, SystemSettings sourceMember, T destMember, ResolutionContext context)
        {
            var value = _settings.GetValue(sourceMember);

            return _converter.Convert(value);
        }
    }
}
