using System;
using System.Linq;
using System.Reflection;

namespace InfraManager.Core
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class TransitionAttribute : Attribute
    {
        #region properties
        public int[] Items { get; private set; }
        #endregion

        #region constructors
        public TransitionAttribute(params int[] items)
        {
            this.Items = items == null ? new int[0] : items;
        }
        #endregion

        #region static method GetItems
        public static T[] GetItems<T>(T item) where T : struct, IConvertible
        {
            FieldInfo fieldInfo = item.GetType().GetField(item.ToString());
            if (fieldInfo == null)
                throw new NotImplementedException();
            //
            object[] attributes = fieldInfo.GetCustomAttributes(typeof(TransitionAttribute), true);
            if (attributes == null || attributes.Length == 0)
                throw new NotImplementedException();
            //
            var a = (TransitionAttribute)attributes[0];
            var retval = a.Items.Select(x => (T)Enum.ToObject(typeof(T), x)).ToArray();
            //
            return retval;
        }
        #endregion
    }
}
