using Inframanager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager
{
    public static class TypeExtensions
    {
        public static bool HasInterface<TInterface>(this Type type)
        {
            return type.GetInterfaces().Any(i => i == typeof(TInterface));
        }

        public static bool HasInterface(this Type type, Type interfaceType)
        {
            return interfaceType.IsGenericType
                ? type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
                : type.GetInterfaces().Any(i => i == interfaceType);
        }

        public static bool HasAttribute<TAttribute>(this Type type)
        {
            return type.CustomAttributes.Any(a => a.AttributeType == typeof(TAttribute));
        }

        public static TAttribute GetAttribute<TAttribute>(this Type type)
        {
            return type.GetAttributes<TAttribute>().FirstOrDefault();
        }

        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this Type type)
        {
            return Attribute.GetCustomAttributes(type, typeof(TAttribute)).Cast<TAttribute>();
        }

        public static bool AllowNull(this Type type)
        {
            return !type.IsValueType
                || Nullable.GetUnderlyingType(type) == null;
        }

        public static ObjectClass? GetObjectClassOrDefault(this Type type)
        {
            var attribute = type.GetObjectClassMappingAttribute();

            return type.GetObjectClassMappingAttribute()?.ObjectClass;
        }

        public static ObjectClass GetObjectClassOrRaiseError(this Type type)
        {
            return type.GetObjectClassOrDefault() 
                ?? throw new NotSupportedException($"Type ({type}) should have ObjectClassMappingAttribute.");
        }

        private static ObjectClassMappingAttribute GetObjectClassMappingAttribute(this Type type)
        {
            return type.GetCustomAttributes(typeof(ObjectClassMappingAttribute), true).FirstOrDefault() as ObjectClassMappingAttribute;
        }
    }
}
