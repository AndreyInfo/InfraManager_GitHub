
namespace InfraManager.Core
{
	public enum TimeInterval : byte
	{
		[FriendlyName("За текущий час")]
		PresentHour = 0,
		[FriendlyName("За последний час (за последние 60 мин.)")]
		LastHour = 1,
		[FriendlyName("За прошедний час")]
		PastHour = 2,

		[FriendlyName("За сегодня")]
		Today = 3,

		[FriendlyName("За вчера")]
		Yesterday = 4,

		[FriendlyName("За текущую неделю")]
		PresentWeek = 5,
		[FriendlyName("За последнюю неделю (за последние 7 дней)")]
		LastWeek = 6,
		[FriendlyName("За прошедшую неделю")]
		PastWeek = 7,

		[FriendlyName("За текущий месяц")]
		PresentMonth = 8,
		[FriendlyName("За последний месяц (за последние 30 дней)")]
		LastMonth = 9,
		[FriendlyName("За прошедший месяц")]
		PastMonth = 10,

		[FriendlyName("За текущий квартал")]
		PresentQuarter = 11,
		[FriendlyName("За последний квартал (за последние 3 мес.)")]
		LastQuarter = 12,
		[FriendlyName("За прошедший квартал")]
		PastQuarter = 13,

		[FriendlyName("За текущий год")]
		PresentYear = 14,
		[FriendlyName("За последний год (за последние 365 дней)")]
		LastYear = 15,
		[FriendlyName("За прошедший год")]
		PastYear = 16
	}
}
