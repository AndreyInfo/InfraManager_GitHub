using System;

namespace InfraManager.Core
{
	public static class EventArgs
	{
		#region method Create
		public static EventArgs<T1> Create<T1>(T1 value1)
		{
			return new EventArgs<T1>(value1);
		}

		public static EventArgs<T1, T2> Create<T1, T2>(T1 value1, T2 value2)
		{
			return new EventArgs<T1, T2>(value1, value2);
		}

		public static EventArgs<T1, T2, T3> Create<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
		{
			return new EventArgs<T1, T2, T3>(value1, value2, value3);
		}
		#endregion
	}

	public class EventArgs<T1> : System.EventArgs
	{
		#region properties
		public T1 Value1 { get; private set; }
		#endregion


		#region constructors
		public EventArgs(T1 value1)
		{
			this.Value1 = value1;
		}
		#endregion
	}

	public class EventArgs<T1, T2> : EventArgs<T1>
	{
		#region properties
		public T2 Value2 { get; private set; }
		#endregion


		#region constructors
		public EventArgs(T1 value1, T2 value2)
			: base(value1)
		{
			this.Value2 = value2;
		}
		#endregion
	}

	public class EventArgs<T1, T2, T3> : EventArgs<T1, T2>
	{
		#region properties
		public T3 Value3 { get; private set; }
		#endregion


		#region constructors
		public EventArgs(T1 value1, T2 value2, T3 value3)
			: base(value1, value2)
		{
			this.Value3 = value3;
		}
		#endregion
	}
}
