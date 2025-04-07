using System;

namespace InfraManager.BLL.Settings.SettingConverters
{
    internal class ByteArraySettingValueConverter :
        SettingValueConverter<byte[]>,
        ISelfRegisteredService<IConvertSettingValue>,
        ISelfRegisteredService<IConvertSettingValue<byte[]>>
    {
        public override byte[] Convert(byte[] value)
        {
            return value;
        }

        public override byte[] ConvertBack(byte[] value)
        {
            return value;
        }
    }
}
