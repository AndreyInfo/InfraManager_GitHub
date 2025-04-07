using System;

namespace InfraManager.Core
{
	[Serializable]
    public enum PolicyType : byte
    {
        [FriendlyName("Разрешить")]
        Allow,

        [FriendlyName("Запретить")]
        Deny,
    }
}
