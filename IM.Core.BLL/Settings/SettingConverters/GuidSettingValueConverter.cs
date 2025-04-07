using System;
using System.Text;

namespace InfraManager.BLL.Settings.SettingConverters
{
    internal class GuidSettingValueConverter :
        SettingValueConverter<Guid?>,
        ISelfRegisteredService<IConvertSettingValue>,
        ISelfRegisteredService<IConvertSettingValue<Guid?>>
    {
        public override Guid? Convert(byte[] value)
        {
            if (value == null || value.Length == 0)
            {
                return null;
            }

            return Guid.TryParse(Encoding.UTF8.GetString(value), out var guid)
                ? guid
                : throw new InvalidCastException($"Setting value {Encoding.UTF8.GetString(value)} is not valid GUID");
        }

        public override byte[] ConvertBack(Guid? value)
        {
            return value.HasValue 
                ? Encoding.UTF8.GetBytes(value.ToString())
                : Array.Empty<byte>();
        }

        protected override Guid? ConvertValue(object value)
        {
            if (value is null)
            {
                return null;
            }
            else 
            {
                return Guid.TryParse(value.ToString(), out var guid) ? guid : throw new InvalidObjectException("Ошибка. Получено неверное значение");
            }
        }
    }
}
