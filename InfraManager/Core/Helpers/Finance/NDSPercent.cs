
using System;

namespace InfraManager.Core.Helpers.Finance
{
    [Serializable]
    public enum NDSPercent : byte
    {
        [FriendlyName("Вручную"), FriendlyName("Custom", "en-US")]
        Custom = 0,
        [FriendlyName("7"), FriendlyName("7", "en-US")]
        Seven = 1,
        [FriendlyName("10"), FriendlyName("10", "en-US")]
        Ten = 2,
        [FriendlyName("18"), FriendlyName("18", "en-US")]
        Eighteen = 3,
        [FriendlyName("20"), FriendlyName("20", "en-US")]
        Twenty = 4
    }
}
