using System;

namespace InfraManager.BLL.Settings.SettingConverters
{
    internal class IntSettingValueConverter : 
        SettingValueConverter<int>,
        ISelfRegisteredService<IConvertSettingValue>,
        ISelfRegisteredService<IConvertSettingValue<int>>
    {
        public override int Convert(byte[] settingValue)
        {
			if (settingValue != null && settingValue.Length == 4)
			{
				return (settingValue[0] << 24) |
					   (settingValue[1] << 16) |
					   (settingValue[2] << 8) |
					   (settingValue[3]);
			}
			else throw new ArgumentException(nameof(settingValue));
		}

        public override byte[] ConvertBack(int value)
        {
            return new byte[] 
            {
                (byte)(value >> 24),
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)value
            };
        }
    }
}
