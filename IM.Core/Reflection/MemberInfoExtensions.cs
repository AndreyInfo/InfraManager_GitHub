using System;
using System.Linq;
using System.Reflection;

namespace InfraManager
{
    public static class MemberInfoExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this MemberInfo property)
            where TAttribute : Attribute
        {
            return Attribute
                .GetCustomAttributes(property, typeof(TAttribute))
                .Cast<TAttribute>()
                .FirstOrDefault();
        }

        public static bool HasAttribute<TAttribute>(this MemberInfo property)
        {
            return Attribute.IsDefined(property, typeof(TAttribute));
        }

        public static bool CanBeEmpty(this PropertyInfo property)
        {
            return property.PropertyType.AllowNull();
        }
    }
}
