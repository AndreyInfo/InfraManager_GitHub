using System;

namespace InfraManager.BLL.Settings.SettingConverters
{
    internal class ByteSettingValueConverter :
        SettingValueConverter<byte>,
        ISelfRegisteredService<IConvertSettingValue>,
        ISelfRegisteredService<IConvertSettingValue<byte>>
    {
        public override byte Convert(byte[] value)
        {
            if (value != null && value.Length == 1)
            {
                return value[0];
            }
            else throw new ArgumentException(nameof(value));
        }

        public override byte[] ConvertBack(byte value)
        {
            return new[] { value };
        }
    }
}
