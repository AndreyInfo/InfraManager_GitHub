
namespace InfraManager.Core.Extensions
{
	public static class NumericExtensions
	{
		#region NumeralFormat
		public static string NumeralFormat(this int count, string single, string plural1, string plural2)
		{
			var @string = count.ToString();
			if (int.Parse(@string.Right(2)) >= 10 && int.Parse(@string.Right(2)) <= 19)
				return string.Format(plural2, count);
			else if (int.Parse(@string.Right(1)) >= 2 && int.Parse(@string.Right(1)) <= 4)
				return string.Format(plural1, count);
			else if (int.Parse(@string.Right(1)) == 1)
				return string.Format(single, count);
			else
				return string.Format(plural2, count);
		}
		#endregion
	}
}
