using System;

namespace InfraManager.Core
{
	[Serializable]
	public enum AuthenticationType : byte
	{
		IntID,
		ID,
		Login,
		Email,
	}
}
