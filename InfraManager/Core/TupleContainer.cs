using System;
using System.Reflection;
using InfraManager.Core.Extensions;

namespace InfraManager.Core
{
	public class TupleContainer<TTuple, TItem1>
	{
		#region fields
		protected static readonly Type[] __types = { typeof(Tuple<>), typeof(Tuple<,>), typeof(Tuple<,,>), typeof(Tuple<,,,>), typeof(Tuple<,,,,>) };
		#endregion


		#region properties
		public TTuple Tuple { get; protected set; }

		protected TItem1 Item1 { get; set; }
		#endregion


		#region constructors
		protected TupleContainer() { }
		#endregion


		#region override method ToString
		public override string ToString()
		{
			return this.Item1 == null ? string.Empty : this.Item1.ToString();
		}
		#endregion

		#region override method Equals
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (object.ReferenceEquals(this, obj)) return true;
			if (this.GetType() != obj.GetType()) return false;
			TupleContainer<TTuple, TItem1> other = (TupleContainer<TTuple, TItem1>)obj;
			return this.Tuple.Equals(other.Tuple);
		}
		#endregion

		#region override method GetHashCode
		public override int GetHashCode()
		{
			return this.Tuple.GetHashCode();
		}
		#endregion


		#region operators
		public static explicit operator TupleContainer<TTuple, TItem1>(TTuple tuple)
		{
			var type = typeof(TTuple);
			if (!type.IsGenericType)
				return null;
			var genericTypeDefinition = type.GetGenericTypeDefinition();
			if (!__types.Contains(x => x == genericTypeDefinition))
				return null;
			if (typeof(TItem1) != type.GetGenericArguments()[0])
				return null;
			return new TupleContainer<TTuple, TItem1> { Tuple = tuple, Item1 = (TItem1)type.InvokeMember("Item1", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, tuple, new object[] { }) };
		}
		#endregion
	}

	public class TupleContainer<TTuple> : TupleContainer<TTuple, string>
	{
		#region operators
		public static explicit operator TupleContainer<TTuple>(TTuple tuple)
		{
			var type = typeof(TTuple);
			if (!type.IsGenericType)
				return null;
			var genericTypeDefinition = type.GetGenericTypeDefinition();
			if (!__types.Contains(x => x == genericTypeDefinition))
				return null;
			if (typeof(string) != type.GetGenericArguments()[0])
				return null;
			return new TupleContainer<TTuple> { Tuple = tuple, Item1 = (string)type.InvokeMember("Item1", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, tuple, new object[] { }) };
		}
		#endregion
	}
}