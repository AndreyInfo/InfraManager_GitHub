using System;

namespace InfraManager.BLL.Settings.SettingConverters
{
    internal class BooleanSettingValueConverter : 
        SettingValueConverter<bool>,
        ISelfRegisteredService<IConvertSettingValue>,
        ISelfRegisteredService<IConvertSettingValue<bool>>
    {
        public override bool Convert(byte[] settingValue)
        {
            if (settingValue != null && settingValue.Length == 1)
            {
                return settingValue[0] == 1;
            }
            else throw new ArgumentException(nameof(settingValue));
        }

        public override byte[] ConvertBack(bool value)
        {
            return new[] { (byte)(value ? 1 : 0) };
        }
    }
}
