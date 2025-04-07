using System;

namespace InfraManager.Core.Extensions
{
	public static class ObjectExtensions
	{
		#region GetValueOrDefault
		public static T GetValueOrDefault<T>(this object source)
		{
			if (source == DBNull.Value)
				return default(T);
			else
				return (T)source;
		}
		#endregion

		#region GetValueOrDBNull
		public static object GetValueOrDBNull<T>(this T source)
			where T : class
		{
			if (source == null)
				return DBNull.Value;
			else
				return source;
		}

		public static object GetValueOrDBNull<T>(this Nullable<T> source)
			where T : struct
		{
			if (source.HasValue)
				return source.GetValueOrDefault();
			else
				return DBNull.Value;
		}
		#endregion

		#region ToStringOrEmpty
		public static string ToStringOrEmpty(this object source)
		{
			return source == null ? string.Empty : source.ToString();
		}
		#endregion
	}
}
