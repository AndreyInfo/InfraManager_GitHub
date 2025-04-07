
namespace InfraManager.Core
{
	public enum FilterOperation
	{
		[FriendlyName("=")]
		Equal = 1,
		[FriendlyName("<>")]
		NotEqual = 2,
		[FriendlyName("Like")]
		Like = 4,
		[FriendlyName("<")]
		LT = 8,
		[FriendlyName("<=")]
		LTE = 16,
		[FriendlyName(">=")]
		GTE = 32,
		[FriendlyName(">")]
		GT = 64
	}
}
