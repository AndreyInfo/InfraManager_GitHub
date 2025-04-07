using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace InfraManager.Core
{
	public class FEqualityComparer<T> : IEqualityComparer<T>
	{
		private readonly Func<T, T, bool> _comparer;
		private readonly Func<T, int> _hasher;


		public FEqualityComparer(Func<T, T, bool> comparer)
			: this(comparer, t => t.GetHashCode())
		{ }

		public FEqualityComparer(Func<T, T, bool> comparer, Func<T, int> hasher)
		{
			#region assertions
			Debug.Assert(comparer != null);
			Debug.Assert(hasher != null);
			#endregion
			//
			_comparer = comparer;
			_hasher = hasher;
		}


		#region interface IEqualityComparer
		public bool Equals(T x, T y)
		{
			return _comparer(x, y);
		}

		public int GetHashCode(T obj)
		{
			return _hasher(obj);
		}
		#endregion
	}
}
