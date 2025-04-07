using System;

namespace InfraManager.Core.Extensions
{
	public static class CharExtensions
	{
		#region IsDigit
		public static bool IsDigit(this char c)
		{
			return Char.IsDigit(c);
		}
		#endregion

		#region IsHex
		public static bool IsHex(this char c)
		{
			return (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
		}
		#endregion

		#region IsLetter
		public static bool IsLetter(this char c)
		{
			return Char.IsLetter(c);
		}
		#endregion

		#region IsLetterOrDigit
		public static bool IsLetterOrDigit(this char c)
		{
			return Char.IsLetterOrDigit(c);
		}
		#endregion
	}
}
