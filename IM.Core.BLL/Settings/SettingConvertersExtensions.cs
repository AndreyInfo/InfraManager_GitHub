using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.BLL.Settings
{
    internal static class SettingConvertersExtensions
    {
        public static dynamic Convert(
            this IEnumerable<IConvertSettingValue> converters,
            Type settingType,
            byte[] settingValue)
        {
            return converters.Find(settingType)?.Convert(settingValue) 
                ?? throw new NotSupportedException($"Setting convertion to {settingType} is not supported.");
        }

        public static dynamic Convert<T>(
            this IEnumerable<IConvertSettingValue> converters,
            byte[] settingValue)
        {
            return (T)converters.Convert(typeof(T), settingValue);
        }

        public static byte[] ConvertBack(
            this IEnumerable<IConvertSettingValue> converters,
            object value)
        {
            return converters.Find(value.GetType())?.ConvertBack(value)
                ?? throw new NotSupportedException($"Setting convertion from {value.GetType()} is not supported.");
        }

        public static IConvertSettingValue Find(
            this IEnumerable<IConvertSettingValue> converters,
            Type valueType)
        {
            return converters.FirstOrDefault(x => x.Supports(valueType));
        }
    }
}
