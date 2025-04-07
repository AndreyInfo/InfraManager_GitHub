using System;

namespace InfraManager.BLL.Settings.SettingConverters
{
    internal class LongSettingValueConverter :
        SettingValueConverter<long>,
        ISelfRegisteredService<IConvertSettingValue>,
        ISelfRegisteredService<IConvertSettingValue<long>>
    {
        public override long Convert(byte[] value)
        {
            if (value != null && value.Length == 8)
            {
                return ((long)value[0] << 56) |
                       ((long)value[1] << 48) |
                       ((long)value[2] << 40) |
                       ((long)value[3] << 32) |
                       ((long)value[4] << 24) |
                       ((long)value[5] << 16) |
                       ((long)value[6] << 8) |
                       ((long)value[7]);
            }
            else throw new ArgumentException(nameof(value));
        }

        public override byte[] ConvertBack(long value)
        {
            return new[]
            {
                (byte)(value >> 56),
                (byte)(value >> 48),
                (byte)(value >> 40),
                (byte)(value >> 32),
                (byte)(value >> 24),
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)(value)
            };
        }
    }
}
