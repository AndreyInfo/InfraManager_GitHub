using System;

namespace InfraManager.Core.Extensions
{
	public static class MonadicExtensions
	{
		#region Maybe
		public static T2 Maybe<T1, T2>(this T1 o, Func<T1, T2> func)
			where T1 : class
			where T2 : class
		{
			if (o == null) return null;
			return func(o);
		}
		#endregion

		#region Default
		public static T2 Default<T1, T2>(this T1 o, Func<T1, T2> func, T2 @default)
			where T1 : class
			where T2 : class
		{
			if (o == null) return @default;
			return func(o);
		}
		#endregion

		#region If
		public static T If<T>(this T o, Func<T, bool> func)
			where T : class
		{
			if (o == null) return null;
			return func(o) ? o : null;
		}
		#endregion

		#region Unless
		public static T Unless<T>(this T o, Func<T, bool> func)
			where T : class
		{
			if (o == null) return null;
			return func(o) ? null : o;
		}
		#endregion

		#region Do
		public static T Do<T>(this T o, Action<T> action)
			where T : class
		{
			if (o == null) return null;
			action(o);
			return o;
		}

		public static T Do<T>(this T o, params Action<T>[] actions)
			where T : class
		{
			if (o == null) return null;
			foreach (var action in actions) action(o);
			return o;
		}
		#endregion
	}
}
