using System;

namespace InfraManager.Core.Logging
{
	[Serializable]
	public enum FilterResult : sbyte
	{
		Reject = - 1,
		Ignore = 0,
		Accept = 1,
	}
}
