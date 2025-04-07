using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;

namespace InfraManager.Core.Helpers
{
    public static class TypeHelper
    {
        #region fields
        private static readonly ConcurrentDictionary<Type, string> __friendlyClassNames = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<Type, Dictionary<string, string>> __friendlyPropertyNames = new ConcurrentDictionary<Type, Dictionary<string, string>>();
        private static readonly ConcurrentDictionary<Enum, List<FriendlyNameAttribute>> __friendlyEnumFieldAttributes = new ConcurrentDictionary<Enum, List<FriendlyNameAttribute>>();
        private static readonly ConcurrentDictionary<Type, Tuple<string, string, string>> __friendlyActionNames = new ConcurrentDictionary<Type, Tuple<string, string, string>>();
        #endregion


        #region method GetFriendlyClassName
        public static string GetFriendlyClassName(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type", "type is null.");
            //
            string friendlyClassName = null;
            //
            if (!__friendlyClassNames.TryGetValue(type, out friendlyClassName))
            {
                object[] attributes = type.GetCustomAttributes(typeof(FriendlyNameAttribute), true);
                if (attributes.Length > 0)
                    friendlyClassName = ((FriendlyNameAttribute)attributes[0]).Name;
                if (friendlyClassName == null)
                    friendlyClassName = string.Empty;
                __friendlyClassNames.TryAdd(type, friendlyClassName);
            }
            //
            return friendlyClassName;
        }
        #endregion

        #region method GetEnumByAnotherEnumWithName
        public static T GetEnumByAnotherEnumWithName<T>(Type type, string propertyName) where T : System.Enum
        {
            var property = GetEnumFieldValue(type, propertyName);
            var name = property.ToString();
            var result = Enum.Parse(typeof(T), name, true);
            return (T)result;
        }
        #endregion


        #region method GetFriendlyPropertyName
        public static string GetFriendlyPropertyName(Type type, string propertyName)
        {
            if (type == null)
                throw new ArgumentNullException("type", "type is null.");
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("propertyName is null or empty.", "propertyName");
            //
            Dictionary<string, string> friendlyPropertyNames = null;
            if (!__friendlyPropertyNames.TryGetValue(type, out friendlyPropertyNames))
            {
                friendlyPropertyNames = new Dictionary<string, string>();
                __friendlyPropertyNames.AddOrUpdate(type, (x) => friendlyPropertyNames, (x, oldVal) => friendlyPropertyNames);
            }
            string friendlyPropertyName = null;
            if (!friendlyPropertyNames.TryGetValue(propertyName, out friendlyPropertyName))
            {
                PropertyInfo propertyInfo = type.GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    object[] attributes = propertyInfo.GetCustomAttributes(typeof(FriendlyNameAttribute), true);
                    if (attributes.Length > 0)
                        friendlyPropertyName = ((FriendlyNameAttribute)attributes[0]).Name;
                }
                if (friendlyPropertyName == null)
                    friendlyPropertyName = string.Empty;
                friendlyPropertyNames[propertyName] = friendlyPropertyName;
            }
            //
            return friendlyPropertyName;
        }
        #endregion

        #region method GetFriendlyEnumFieldName
        public static string GetFriendlyEnumFieldName(Enum enumField)
        {
            List<FriendlyNameAttribute> friendlyNameArrtibutes = null;
            if (!__friendlyEnumFieldAttributes.TryGetValue(enumField, out friendlyNameArrtibutes))
            {
                FieldInfo fieldInfo = enumField.GetType().GetField(enumField.ToString());
                if (fieldInfo != null)
                {
                    object[] attributes = fieldInfo.GetCustomAttributes(typeof(FriendlyNameAttribute), true);
                    if (attributes.Length > 0)
                    {
                        friendlyNameArrtibutes = new List<FriendlyNameAttribute>();
                        foreach (FriendlyNameAttribute att in attributes)
                            friendlyNameArrtibutes.Add((FriendlyNameAttribute)att);
                    }
                }
                if (friendlyNameArrtibutes == null)
                    friendlyNameArrtibutes = new List<FriendlyNameAttribute>();
                __friendlyEnumFieldAttributes.TryAdd(enumField, friendlyNameArrtibutes);
            }
            //
            if (friendlyNameArrtibutes.Count == 0)
                return string.Empty;
            else if (friendlyNameArrtibutes.Count == 1)
                return friendlyNameArrtibutes[0].Name;
            else
            {
                string currentCulture = CultureInfo.CurrentCulture.Name;
                FriendlyNameAttribute att;
                //if (ApplicationManager.Instance.IsWebApplication) 
                //{
                    att = friendlyNameArrtibutes.Find(x => x.CultureName == currentCulture);
                    if (att != null)
                        return att.Name;//name by culture
                //}
                //
                att = friendlyNameArrtibutes.Find(x => x.CultureName == string.Empty);
                if (att != null)
                    return att.Name;//name by default culture
                //
                return friendlyNameArrtibutes[0].Name;
            }
        }

        public static string GetFriendlyEnumFieldName(Enum enumField, String locale)
        {
            List<FriendlyNameAttribute> friendlyNameArrtibutes = null;
            if (!__friendlyEnumFieldAttributes.TryGetValue(enumField, out friendlyNameArrtibutes))
            {
                FieldInfo fieldInfo = enumField.GetType().GetField(enumField.ToString());
                if (fieldInfo != null)
                {
                    object[] attributes = fieldInfo.GetCustomAttributes(typeof(FriendlyNameAttribute), true);
                    if (attributes.Length > 0)
                    {
                        friendlyNameArrtibutes = new List<FriendlyNameAttribute>();
                        foreach (FriendlyNameAttribute att in attributes)
                            friendlyNameArrtibutes.Add((FriendlyNameAttribute)att);
                    }
                }
                if (friendlyNameArrtibutes == null)
                    friendlyNameArrtibutes = new List<FriendlyNameAttribute>();
                __friendlyEnumFieldAttributes.TryAdd(enumField, friendlyNameArrtibutes);
            }
            //
            if (friendlyNameArrtibutes.Count == 0)
                return string.Empty;
            else
            {
                FriendlyNameAttribute att;
                att = friendlyNameArrtibutes.Find(x => x.CultureName == locale);
                if (att != null)
                    return att.Name;//name by default culture
                att = friendlyNameArrtibutes.Find(x => x.CultureName == string.Empty);
                if (att != null)
                    return att.Name;//name by default culture
                return friendlyNameArrtibutes[0].Name;
            }
        }
        #endregion

        #region method GetEnumFieldValue
        public static Enum GetEnumFieldValue(Type enumType, string friendlyName)
        {
            foreach (FieldInfo fieldInfo in enumType.GetFields())
            {
                object[] attributes = fieldInfo.GetCustomAttributes(typeof(FriendlyNameAttribute), true);
                if (attributes.Length > 0 && ((FriendlyNameAttribute)attributes[0]).Name == friendlyName)
                    return (Enum)fieldInfo.GetValue(null);
            }
            //
            return Undefined.Value;
        }
        #endregion

        #region method GetFriendlyActionName
        public static Tuple<string, string, string> GetFriendlyActionName(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type", "type is null.");
            //
            Tuple<string, string, string> result = null;
            //
            if (!__friendlyActionNames.TryGetValue(type, out result))
            {
                object[] attributes = type.GetCustomAttributes(typeof(FriendlyActionNameAttribute), true);
                if (attributes.Length > 0)
                {
                    FriendlyActionNameAttribute att = (FriendlyActionNameAttribute)attributes[0];
                    result = Tuple.Create(att.AddActionName, att.SaveActionName, att.RemoveActionName);
                }
                if (result == null)
                    result = Tuple.Create(string.Empty, string.Empty, string.Empty);
                //
                __friendlyActionNames.TryAdd(type,  result);
            }
            //
            return result;
        }
        #endregion
    }

    /// <summary>
    /// класс предназначен для динамического получения значений свойств/полей объекта
    /// то есть, на этапе компиляции мы не обязаны знать о свойствах/полях объекта.
    /// </summary>
    /// <example>
    /// <code>
    /// MemberGetDelegate&lt;string&gt; GetName = TypeUtility&lt;Person&gt;.GetMemberGetDelegate&lt;string&gt;("Name");
    /// string name = GetName(new Person("ivan"));
    /// </code>
    /// </example>
    /// <typeparam name="TObject"></typeparam>
    internal sealed class TypeHelper<TObject>
    {
        #region classes
        public delegate TMember MemberGetDelegate<TMember>(TObject obj);
        #endregion


        #region fields
        private static Dictionary<string, Delegate> __cache = new Dictionary<string, Delegate>();
        #endregion


        #region static method GetMemberGetDelegate
        public static MemberGetDelegate<TMember> GetMemberGetDelegate<TMember>(string memberName)
        {
            Type objectType = typeof(TObject);
            Type memberType = typeof(TMember);
            //
            PropertyInfo pi = objectType.GetProperty(memberName);
            if (pi != null)
            {
                //работаем со свойством
                MethodInfo mi = pi.GetGetMethod();
                //
                if (mi != null)
                {
                    Delegate del = Delegate.CreateDelegate(
                        typeof(MemberGetDelegate<TMember>), mi, false);
                    //
                    if (del != null)
                        return (MemberGetDelegate<TMember>)del;
                    else
                    {
                        DynamicMethod dynamicMethod = new DynamicMethod(
                            string.Concat("get_", memberName),
                            memberType,
                            new Type[] { objectType },
                            objectType,
                            true);
                        ILGenerator il = dynamicMethod.GetILGenerator();
                        //
                        if (mi.IsStatic)
                            il.EmitCall(OpCodes.Call, mi, null);
                        else
                        {
                            if (!objectType.IsClass)
                            {
                                il.Emit(OpCodes.Ldarga_S, 0);
                                il.EmitCall(OpCodes.Call, mi, null);
                            }
                            else
                            {
                                il.Emit(OpCodes.Ldarg_0);
                                il.EmitCall(OpCodes.Callvirt, mi, null);
                            }
                        }
                        if (!pi.PropertyType.IsClass && memberType.IsClass)
                            il.Emit(OpCodes.Box, pi.PropertyType);
                        il.Emit(OpCodes.Ret);
                        //
                        return (MemberGetDelegate<TMember>)dynamicMethod.CreateDelegate(typeof(MemberGetDelegate<TMember>));
                    }
                }
                else
                {
                    throw new Exception(string.Format("Property '{0}' of type '{1}' doesn't have a public Get accessor.",
                        memberName,
                        objectType.Name));
                }
            }
            //
            FieldInfo fi = objectType.GetField(memberName);
            if (fi != null)
            {
                //работаем с полем
                DynamicMethod dynamicMethod = new DynamicMethod(
                    string.Concat("get_", memberName),
                    typeof(TMember),
                    new Type[] { objectType },
                    objectType,
                    true);

                ILGenerator il = dynamicMethod.GetILGenerator();
                //помещаем объект (argument 0) в стек
                il.Emit(OpCodes.Ldarg_0);
                //помещаем значение поля объекта (fi) в стек 
                il.Emit(OpCodes.Ldfld, fi);
                //добавим (если надо) boxing
                if (!fi.FieldType.IsClass && memberType.IsClass)
                    il.Emit(OpCodes.Box, fi.FieldType);
                //возвращаем требуемое значение (в вершине стека)
                il.Emit(OpCodes.Ret);

                return (MemberGetDelegate<TMember>)dynamicMethod.CreateDelegate(
                        typeof(MemberGetDelegate<TMember>));
            }
            //
            throw new Exception(string.Format("Member '{0}' is not a Public property or field of type '{1}'",
                memberName,
                objectType.Name));
        }
        #endregion

        #region static method GetCachedMemberGetDelegate
        public static MemberGetDelegate<TMember> GetCachedMemberGetDelegate<TMember>(string memberName)
        {
            Delegate retval = null;
            //
            if (__cache.TryGetValue(memberName, out retval))
                return (MemberGetDelegate<TMember>)retval;
            else
                lock (__cache)
                {
                    if(__cache.ContainsKey(memberName))
                        return (MemberGetDelegate<TMember>)__cache[memberName];
                    //
                    retval = GetMemberGetDelegate<TMember>(memberName);
                    __cache.Add(memberName, retval);
                    return (MemberGetDelegate<TMember>)retval;
                }
        }
        #endregion
    }
}