using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Runtime.Serialization;

namespace InfraManager.Core
{
    public static class Filter
    {
        internal readonly static string Version = "1.1";

        #region method GetStringRepresentation
        public static string GetStringRepresentation(object @object, string format)
        {
            if (@object == null)
                return string.Empty;
            //
            var type = @object.GetType();
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                if (format == null)
                    return ((DateTime)@object).ToString(Global.DateTimeFormat);
                else
                    return ((DateTime)@object).ToString(format);
            }
            else if (type == typeof(Decimal) || type == typeof(Decimal?))
            {
                if (format == null)
                    return ((Decimal)@object).ToString(Global.DecimalFormat);
                else
                    return ((Decimal)@object).ToString(format);
            }
            else
            {
                return @object.ToString();
            }
        }
        #endregion

        #region static method Deserialize
        public static Object Deserialize(byte[] data)
        {
            if (data == null)
                return null;
            //
            SerializationBinder binder = null;
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader br = new BinaryReader(ms))
            {
                string nullString = Convert.ToChar(0).ToString();
                //
                var ver = br.ReadString();
                if (ver == "1.0")
                    binder = new SoftIntegroConverter();
                else if (ver != Version)
                    throw new NotSupportedException();
                //
                string type = br.ReadString();
                //
                string filterName = br.ReadString();
                if (filterName == nullString)
                    filterName = null;
                //
                string memberName = br.ReadString();
                if (memberName == nullString)
                    memberName = null;
                //
                string format = br.ReadString();
                if (format == nullString)
                    format = null;
                //
                string likableValue = br.ReadString();
                if (likableValue == nullString)
                    likableValue = null;
                //
                FilterOperator fOperator = (FilterOperator)br.ReadInt32();
                FilterOperation fOperation = (FilterOperation)br.ReadInt32();
                //
                object left = null;
                int len = br.ReadInt32();
                if (len > 0)
                {
                    byte[] tmp = br.ReadBytes(len);
                    left = Deserialize(tmp);
                }
                //
                object right = null;
                len = br.ReadInt32();
                if (len > 0)
                {
                    byte[] tmp = br.ReadBytes(len);
                    right = Deserialize(tmp);
                }
                //
                IComparable comparableValue = null;
                len = br.ReadInt32();
                if (len > 0)
                {
                    byte[] tmp = br.ReadBytes(len);
                    using (MemoryStream msComparableValue = new MemoryStream(tmp))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        if (binder != null)
                            bf.Binder = binder;
                        comparableValue = bf.Deserialize(msComparableValue) as IComparable;
                    }
                }
                //
                //
                Type tObject = null;
                if (binder != null)
                {
                    var index = type.IndexOf(", ");
                    var assemblyName = type.Substring(index + 2);
                    var typeName = type.Substring(0, index);
                    //
                    tObject = binder.BindToType(assemblyName, typeName);
                }
                else
                    tObject = Type.GetType(type);
                //
                if (tObject == null)
                    throw new TypeLoadException();
                //						
                Type filterType = typeof(Filter<>);
                Type genericType = filterType.MakeGenericType(new Type[] { tObject });
                ConstructorInfo info = genericType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null,
                    new Type[] { typeof(string), typeof(string), typeof(string), typeof(FilterOperation), typeof(FilterOperator), typeof(object), typeof(object), typeof(string), typeof(IComparable) }, null);
                Object retval = info.Invoke(
                    new object[] { filterName, memberName, format, fOperation, fOperator, left, right, likableValue, comparableValue });
                //
                return retval;
            }
        }
        #endregion

        #region static method Serialize
        public static byte[] Serialize(Object filter)
        {
            if (filter == null)
                return null;
            byte[] retval = (byte[])filter.GetType().InvokeMember("Serialize", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public, null, filter, new object[] { });
            return retval;
        }
        #endregion


        #region convertationZone (SoftIntegro -> InfraManager)
        public sealed class SoftIntegroConverter : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                bool removeIM = (typeName.Contains("SoftIntegro.IM.Net") || typeName.Contains("SoftIntegro.IM.Settings") || typeName.Contains("SoftIntegro.IM.Setup"));
                //
                typeName = typeName.Replace("SoftIntegro", "InfraManager").Replace("Version=5.0.0.0, Culture=neutral, PublicKeyToken=f57227384aa6f133", "Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL");
                assemblyName = assemblyName.Replace("SoftIntegro", "InfraManager").Replace("Version=5.0.0.0, Culture=neutral, PublicKeyToken=f57227384aa6f133", "Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL");
                //
                string typeString = string.Format("{0}, {1}", typeName, assemblyName);
                if (removeIM)
                    typeString = typeString.Replace(".IM", "");
                //
                return Type.GetType(typeString);
            }
        }
        #endregion
    }

    [Serializable]
    public sealed class Filter<T>
    {//Custom serialization present!!! Don't change types!!!
        #region fields
        private string[] _likableParts;
        #endregion


        #region properties
        public string FilterName { get; set; }

        public string MemberName { get; private set; }

        public string Format { get; private set; }

        public FilterOperation FilterOperation { get; private set; }

        public FilterOperator FilterOperator { get; private set; }

        public Filter<T> Left { get; private set; }

        public Filter<T> Right { get; private set; }

        public string LikableValue { get; private set; }

        public IComparable ComparableValue { get; private set; }
        #endregion


        #region constructors
        private Filter()
        { }

        private Filter(string filterName, string memberName, string format, FilterOperation fOperation,
            FilterOperator fOperator, object left, object right, string likableValue, IComparable comparableValue)
        {//for deserialization
            this.FilterName = filterName;
            this.MemberName = memberName;
            this.Format = format;
            this.FilterOperation = fOperation;
            this.FilterOperator = fOperator;
            this.Left = left as Filter<T>;
            this.Right = right as Filter<T>;
            this.LikableValue = likableValue;
            this.ComparableValue = comparableValue;
            if (likableValue != null)
                _likableParts = likableValue.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private Filter(string memberName, FilterOperation filterOperation, IComparable comparableValue, string likableValue, string format)
        {
            this.MemberName = memberName;
            this.Format = format;
            this.FilterOperation = filterOperation;
            this.ComparableValue = comparableValue;
            this.LikableValue = likableValue;
            _likableParts = likableValue.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
        }
        #endregion


        #region static method ConstructFilter
        public static Filter<T> ConstructFilter(string memberName, FilterOperation operation, IComparable comparableValue, string likableValue, string format)
        {
            if (likableValue == null ||
                operation == FilterOperation.Like && (
                    !likableValue.Contains("*") || (
                        likableValue != "*" &&
                        likableValue.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries).Length == 0)))
                throw new NotSupportedException();
            //    
            return new Filter<T>(memberName, operation, comparableValue, likableValue, format);
        }
        #endregion

        #region method Serialize
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                string nullString = Convert.ToChar(0).ToString();
                //
                bw.Write(Filter.Version);
                //
                bw.Write(typeof(T).AssemblyQualifiedName);
                //
                bw.Write(this.FilterName ?? nullString);
                bw.Write(this.MemberName ?? nullString);
                bw.Write(this.Format ?? nullString);
                bw.Write(this.LikableValue ?? nullString);
                //
                bw.Write((int)this.FilterOperator);
                bw.Write((int)this.FilterOperation);
                //
                if (this.Left == null)
                    bw.Write((int)0);
                else
                {
                    byte[] leftPart = this.Left.Serialize();
                    bw.Write(leftPart.Length);
                    bw.Write(leftPart);
                }
                //
                if (this.Right == null)
                    bw.Write((int)0);
                else
                {
                    byte[] rightPart = this.Right.Serialize();
                    bw.Write(rightPart.Length);
                    bw.Write(rightPart);
                }
                //
                if (this.ComparableValue == null)
                    bw.Write((int)0);
                else
                    using (MemoryStream msComparableValue = new MemoryStream())
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(msComparableValue, this.ComparableValue);
                        //
                        byte[] data = msComparableValue.ToArray();
                        bw.Write(data.Length);
                        bw.Write(data);
                    }
                //
                return ms.ToArray();
            }
        }
        #endregion

        #region method Eval
        public bool Eval(T @object)
        {
            //составной фильтр
            if (this.Left != null)
                switch (this.FilterOperator)
                {
                    case FilterOperator.AND:
                        if (this.Left.Eval(@object))
                            return this.Right.Eval(@object);
                        else
                            return false;
                    case FilterOperator.OR:
                        if (this.Left.Eval(@object))
                            return true;
                        else
                            return this.Right.Eval(@object);
                    case FilterOperator.NOT:
                        return !this.Left.Eval(@object);
                }
            //примитивный фильтр
            else
            {
                var memberValue = Helpers.TypeHelper<T>.GetCachedMemberGetDelegate<object>(this.MemberName)(@object);
                //
                switch (this.FilterOperation)
                {
                    case FilterOperation.Equal:
                        if (this.ComparableValue == null)
                        {
                            if (this.LikableValue == string.Empty)
                                return memberValue == null;
                            else
                                return this.LikableValue == Filter.GetStringRepresentation(memberValue, this.Format);
                        }
                        else
                            return ((IComparable)this.ComparableValue).CompareTo(memberValue) == 0;
                    case FilterOperation.NotEqual:
                        if (this.ComparableValue == null)
                        {
                            if (this.LikableValue == string.Empty)
                                return memberValue != null;
                            else
                                return this.LikableValue != Filter.GetStringRepresentation(memberValue, this.Format);
                        }
                        else
                            return ((IComparable)this.ComparableValue).CompareTo(memberValue) != 0;
                    case FilterOperation.LT:
                        if (this.ComparableValue == null)
                        {
                            if (this.LikableValue == string.Empty)
                                return false;
                            else
                                return this.LikableValue.CompareTo(Filter.GetStringRepresentation(memberValue, this.Format)) > 0;
                        }
                        else
                            return ((IComparable)this.ComparableValue).CompareTo(memberValue) > 0;
                    case FilterOperation.LTE:
                        if (this.ComparableValue == null)
                        {
                            if (this.LikableValue == string.Empty)
                                return memberValue == null;
                            else
                                return this.LikableValue.CompareTo(Filter.GetStringRepresentation(memberValue, this.Format)) >= 0;
                        }
                        else
                            return ((IComparable)this.ComparableValue).CompareTo(memberValue) >= 0;
                    case FilterOperation.GT:
                        if (this.ComparableValue == null)
                        {
                            if (this.LikableValue == string.Empty)
                                return false;
                            else
                                return this.LikableValue.CompareTo(Filter.GetStringRepresentation(memberValue, this.Format)) < 0;
                        }
                        else
                            return ((IComparable)this.ComparableValue).CompareTo(memberValue) < 0;
                    case FilterOperation.GTE:
                        if (this.ComparableValue == null)
                        {
                            if (this.LikableValue == string.Empty)
                                return memberValue == null;
                            else
                                return this.LikableValue.CompareTo(Filter.GetStringRepresentation(memberValue, this.Format)) <= 0;
                        }
                        else
                            return ((IComparable)this.ComparableValue).CompareTo(memberValue) <= 0;
                    case FilterOperation.Like:
                        //_likableValue Обязательно содержит *
                        string value = Filter.GetStringRepresentation(memberValue, this.Format);
                        //
                        if (this.LikableValue == "*")
                            return true;
                        //
                        int index = -1;
                        //
                        if (this.LikableValue[0] == '*')
                            index = value.IndexOf(_likableParts[0], StringComparison.CurrentCultureIgnoreCase);
                        else if (value.StartsWith(_likableParts[0], StringComparison.CurrentCultureIgnoreCase))
                            index = 0;

                        for (int i = 1; i < _likableParts.Length - 1; i++)
                        {
                            if (index >= 0)
                                value = value.Substring(index + 1);
                            else
                                return false;
                            //
                            index = value.IndexOf(_likableParts[i], StringComparison.CurrentCultureIgnoreCase);
                        }

                        if (index >= 0)
                        {
                            if (this.LikableValue[this.LikableValue.Length - 1] == '*')
                                return value.IndexOf(_likableParts[_likableParts.Length - 1], StringComparison.CurrentCultureIgnoreCase) >= 0;
                            else if (value.EndsWith(_likableParts[_likableParts.Length - 1], StringComparison.CurrentCultureIgnoreCase))
                                return true;
                            //
                            return false;
                        }
                        else
                            return false;
                }
            }
            //
            throw null;
        }
        #endregion

        #region override method ToString
        public override string ToString()
        {
            return this.FilterName;
        }
        #endregion

        #region override method GetHashCode
        public override int GetHashCode()
        {
            return this.FilterName.GetHashCode();
        }
        #endregion

        #region override method Equals
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (object.ReferenceEquals(this, obj)) return true;
            if (this.GetType() != obj.GetType()) return false;
            Filter<T> other = (Filter<T>)obj;
            return this.FilterName == other.FilterName;
        }
        #endregion


        #region operators
        public static Filter<T> operator &(Filter<T> f1, Filter<T> f2)
        {
            if (f1 == null)
                throw new ArgumentNullException("f1", "left filter is null.");
            if (f2 == null)
                throw new ArgumentNullException("f2", "right filter is null.");
            //
            Filter<T> result = new Filter<T>();
            result.Left = f1;
            result.Right = f2;
            result.FilterOperator = FilterOperator.AND;
            //
            return result;
        }

        public static Filter<T> operator |(Filter<T> f1, Filter<T> f2)
        {
            if (f1 == null)
                throw new ArgumentNullException("f1", "left filter is null.");
            if (f2 == null)
                throw new ArgumentNullException("f2", "right filter is null.");
            //
            Filter<T> result = new Filter<T>();
            result.Left = f1;
            result.Right = f2;
            result.FilterOperator = FilterOperator.OR;
            //
            return result;
        }

        public static Filter<T> operator !(Filter<T> f)
        {
            if (f == null)
                throw new ArgumentNullException("f", "filter is null.");
            //
            Filter<T> result = new Filter<T>();
            result.Left = f;
            result.FilterOperator = FilterOperator.NOT;
            //
            return result;
        }
        #endregion
    }
}
