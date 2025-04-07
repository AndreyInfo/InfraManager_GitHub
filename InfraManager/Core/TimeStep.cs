
namespace InfraManager.Core
{
	public enum TimeStep : byte
	{
		[FriendlyName("Минута")]
		Minute = 0,
		[FriendlyName("Час")]
		Hour = 1,
		[FriendlyName("День")]
		Day = 2,
		[FriendlyName("Неделя")]
		Week = 3,
		[FriendlyName("Месяц")]
		Month = 4,
		[FriendlyName("Квартал")]
		Quarter = 5,
		[FriendlyName("Год")]
		Year = 6
	}
}
