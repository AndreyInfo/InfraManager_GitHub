using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.SettingConverters
{
    internal abstract class SettingValueConverter<T> : IConvertSettingValue<T>
    {
        public abstract T Convert(byte[] settingValue);

        public abstract byte[] ConvertBack(T value);

        public bool Supports(Type type)
        {
            return type.IsAssignableTo(typeof(T));
        }

        object IConvertSettingValue.Convert(byte[] settingValue)
        {
            return Convert(settingValue);
        }

        public byte[] ConvertBack(object value)
        {
            if (value == null)
            {
                return null;
            }

            return ConvertBack(value is T typedValue ? typedValue : ConvertValue(value));
        }

        protected virtual T ConvertValue(object value)
        {
            var parameter = Expression.Constant(value, value.GetType());
            var convertFunction = Expression.Lambda<Func<T>>(
                Expression.ConvertChecked(parameter, typeof(T)))
                .Compile();

            return convertFunction();
        }
    }
}
