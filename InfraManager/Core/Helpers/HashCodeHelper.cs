using System.Diagnostics;
using System.Linq;

namespace InfraManager.Core.Helpers
{
	public static class HashCodeHelper
	{
		#region method GetBernsteinHashCode
		public static int GetBernsteinHashCode(params object[] fields)
		{
			unchecked
			{
				const int basis = 0;
				const int prime = 33;
				int result;
				//
				result = fields.Aggregate(basis, (hash, field) => prime * hash + (field ?? 0).GetHashCode());
				return result;
			}
		}
		#endregion

		#region method GetSAXHashCode
		public static int GetSAXHashCode(params object[] fields)
		{
			unchecked
			{
				const int basis = 0;
				int result;
				//
				result = fields.Aggregate(basis, (hash, field) => hash ^ ((hash << 5) + (hash >> 2) + (field ?? 0).GetHashCode()));
				return result;
			}
		}
		#endregion

		#region method GetFNVHashCode
		public static int GetFNVHashCode(params object[] fields)
		{
			unchecked
			{
				const int basis = (int)2166136261;
				const int prime = 16777619;
				int result;
				//
				result = fields.Aggregate(basis, (hash, field) => (prime * hash) ^ (field ?? 0).GetHashCode());
				return result;
			}
		}
		#endregion

		#region method GetFNV1aHashCode
		public static int GetFNV1aHashCode(params object[] fields)
		{
			unchecked
			{
				const int basis = (int)2166136261;
				const int prime = 16777619;
				int result;
				//
				result = fields.Aggregate(basis, (hash, field) => prime * (hash ^ (field ?? 0).GetHashCode()));
				return result;
			}
		}
		#endregion

		#region method GetOATHashCode
		public static int GetOATHashCode(params object[] fields)
		{
			unchecked
			{
				const int basis = 0;
				int result;
				//
				result = fields.Aggregate(basis, (hash, field) =>
					{
						hash += (field ?? 0).GetHashCode();
						hash += hash << 10;
						hash ^= hash >> 6;
						return hash;
					});
				result += result << 3;
				result ^= result >> 11;
				result += result << 15;
				return result;
			}
		}
		#endregion

		#region method GetELFHashCode
		public static int GetELFHashCode(params object[] fields)
		{
			unchecked
			{
				const int basis = 0;
				int result, temp;
				//
				result = fields.Aggregate(basis, (hash, field) =>
				{
					hash = (hash << 4) + (field ?? 0).GetHashCode();
					temp = hash & (int)0xf0000000;
					if (temp != 0)
						hash ^= temp >> 24;
					hash &= ~temp;
					return hash;
				});
				return result;
			}
		}
		#endregion

		#region method GetSkeetHashCode
		public static int GetSkeetHashCode(params object[] fields)
		{
			unchecked
			{
				const int basis = 17;
				const int prime = 23;
				int result;
				//
				result = fields.Aggregate(basis, (hash, field) => prime * hash + (field ?? 0).GetHashCode());
				return result;
			}
		}
		#endregion

		#region method CombineHashCodes
		public static int CombineHashCodes(int h1, int h2)
		{
			return (((h1 << 5) + h1) ^ h2);
		}

		public static int CombineHashCodes(int h1, int h2, int h3)
		{
			return CombineHashCodes(CombineHashCodes(h1, h2), h3);
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4)
		{
			return CombineHashCodes(CombineHashCodes(h1, h2), CombineHashCodes(h3, h4));
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)
		{
			return CombineHashCodes(CombineHashCodes(h1, h2, h3), CombineHashCodes(h2, h5));
		}
		#endregion
	}
}
