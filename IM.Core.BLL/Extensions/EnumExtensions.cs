using InfraManager.BLL.Localization;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InfraManager.BLL.Extensions
{
    [Obsolete("Полезные расширения в IM.Core, остальное под нож")]
    public static class EnumExtensions
    {
        public static string GetDisplayLabel(this Enum value)
        {
            return GetAttribute<DisplayAttribute>(value, x => x.Name);
        }

        public static string GetDescription(this Enum value)
        {
            return GetAttribute<DescriptionAttribute>(value, x => x.Description);
        }

        private static string GetAttribute<TAttribute>(Enum value, Func<TAttribute, string> attributeGetValue) where TAttribute : Attribute
        {
            var memberInfo = value.GetType()?.GetMember(value.ToString())?.FirstOrDefault();
            if (memberInfo != null)
            {
                var attribute = (TAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(TAttribute));
                return attributeGetValue(attribute);
            }

            return Enum.GetName(value.GetType(), value);
        }

        public static List<ListItem> GetEnumListItems(this Enum type)
        {
            var result = new List<ListItem>();
            foreach (var itm in Enum.GetValues(type.GetType()))
            {
                result.Add(new ListItem { Id = Convert.ToInt32(itm), Name = (itm as Enum)?.GetDisplayLabel() ?? itm.ToString() });
            }
            return result;
        }

        /// <summary>
        /// работает только с атрибутом FriendlyNameAttribute
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static List<ListItem> GetEnumListFriendlyName<TEnum>() where TEnum : Enum
        {
            var allNameEnum = Enum.GetValues(typeof(TEnum))
                                .Cast<TEnum>().ToArray();

            var result = new List<ListItem>();

            foreach (var item in allNameEnum)
            {
                var attribute = GetAttributeValue<FriendlyNameAttribute>(item);

                if (attribute is null)
                    continue;

                result.Add(new ListItem { Id = Convert.ToInt32(item), Name = attribute.Name });
            }

            return result;
        }

        private static T GetAttributeValue<T>(in Enum value) where T : Attribute
        {
            var memberInfo = value.GetType()?.GetMember(value.ToString())?.FirstOrDefault();
            var attribute = (T)Attribute.GetCustomAttribute(memberInfo, typeof(T));
            return attribute;
        }

        public static string GetNameFromFriendlyName<TEnum>(TEnum enumID) where TEnum : Enum
        {
            var enumItem = Enum.GetValues(typeof(TEnum))
                               .Cast<TEnum>().FirstOrDefault(c=> c.Equals(enumID));

            var attribute = GetAttributeValue<FriendlyNameAttribute>(enumItem);

            return attribute?.Name ?? string.Empty;
        }
    }
}