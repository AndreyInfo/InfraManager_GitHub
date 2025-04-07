using System;

namespace InfraManager.Core.Extensions
{
	public static class BooleanExtensions
	{
		private static readonly string TRUE_LOWER_STRING = Boolean.TrueString.ToLower();
		private static readonly string FALSE_LOWER_STRING = Boolean.FalseString.ToLower();

		#region ToLowerString
		public static string ToLowerString(this bool b)
		{
			return b == true ? TRUE_LOWER_STRING : FALSE_LOWER_STRING;
		}
		#endregion
	}
}
