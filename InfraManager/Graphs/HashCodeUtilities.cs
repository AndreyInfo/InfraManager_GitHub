using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfraManager.DataStructures.Graphs
{
	static class HashCodeUtilities
	{
		private const Int32 __fnv1Prime32 = 16777619;
		private const Int32 __fnv1Basis32 = unchecked((int)2166136261);
		private const Int64 __fnv1Prime64 = 1099511628211;
		private const Int64 __fnv1Basis64 = unchecked((int)14695981039346656037);

		#region method GetHashCode
		public static Int32 GetHashCode(Int64 x)
		{
			return Combine((Int32)x, (Int32)(((UInt64)x) >> 32));
		}
		#endregion

		#region method Combine
		public static Int32 Combine(Int32 x, Int32 y)
		{
			return Fold(Fold(__fnv1Basis32, x), y);
		}

		public static Int32 Combine(Int32 x, Int32 y, Int32 z)
		{
			return Fold(Fold(Fold(__fnv1Basis32, x), y), z);
		}
		#endregion

		#region private method Fold
		private static Int32 Fold(Int32 hash, byte value)
		{
			return (hash * __fnv1Prime32) ^ (Int32)value;
		}

		private static Int32 Fold(Int32 hash, Int32 value)
		{
			return Fold(Fold(Fold(Fold(hash,
				(byte)value),
				(byte)(((UInt32)value) >> 8)),
				(byte)(((UInt32)value) >> 16)),
				(byte)(((UInt32)value) >> 24));
		}
		#endregion
	}
}
