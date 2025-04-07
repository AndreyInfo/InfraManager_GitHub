using System;

namespace InfraManager.BLL.Settings.SettingConverters
{
    internal class DecimalSettingValueConverter :
        SettingValueConverter<decimal>,
        ISelfRegisteredService<IConvertSettingValue>,
        ISelfRegisteredService<IConvertSettingValue<decimal>>
    {
        public override decimal Convert(byte[] value)
        {
            if (value == null || value.Length != 16)
                throw new ArgumentException(nameof(value));
            else
            {
                var bits = new int[4];
                for (int intIndex = 3; intIndex > -1; intIndex--)
                    bits[3 - intIndex] = ((int)value[4 * intIndex] << 24)
                                        | ((int)value[4 * intIndex + 1] << 16)
                                        | ((int)value[4 * intIndex + 2] << 8)
                                        | ((int)value[4 * intIndex + 3]);
                return new Decimal(bits);
            }
        }

        public override byte[] ConvertBack(decimal value)
        {
            var bits = decimal.GetBits(value);

            return new[]
            {
                (byte)(bits[3] >> 24),
                (byte)(bits[3] >> 16),
                (byte)(bits[3] >> 8),
                (byte)(bits[3]),
                (byte)(bits[2] >> 24),
                (byte)(bits[2] >> 16),
                (byte)(bits[2] >> 8),
                (byte)(bits[2]),
                (byte)(bits[1] >> 24),
                (byte)(bits[1] >> 16),
                (byte)(bits[1] >> 8),
                (byte)(bits[1]),
                (byte)(bits[0] >> 24),
                (byte)(bits[0] >> 16),
                (byte)(bits[0] >> 8),
                (byte)(bits[0])
            };
        }
    }
}
