using System.Text;

namespace InfraManager.BLL.Settings.SettingConverters
{
    internal class StringSettingValueConverter :
        SettingValueConverter<string>,
        ISelfRegisteredService<IConvertSettingValue>,
        ISelfRegisteredService<IConvertSettingValue<string>>
    {
        public override string Convert(byte[] value)
        {
            if (value.Length == 0)
            {
                return null;
            }
            else return Encoding.UTF8.GetString(value);
        }

        public override byte[] ConvertBack(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
    }
}
