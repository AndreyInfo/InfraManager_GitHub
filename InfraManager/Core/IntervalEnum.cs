using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfraManager.Core
{
    public enum IntervalEnum : byte
    {
        [FriendlyName("Однократно")]
        Once = 0,

        [FriendlyName("Ежемесячно")]
        Monthly = 1,

        [FriendlyName("Ежеквартально")]
        Quarterly = 2,

        [FriendlyName("Ежегодно")]
        Annually = 3,
    }
}
