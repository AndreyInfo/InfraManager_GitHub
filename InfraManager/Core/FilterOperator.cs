using System;

namespace InfraManager.Core
{
    [Flags]
    public enum FilterOperator
    {
        [FriendlyName("Не")]
        NOT = 1,
        [FriendlyName("И")]
        AND = 2,
        [FriendlyName("Или")]
        OR = 4
    }
}
