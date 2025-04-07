using InfraManager.BLL.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    public enum SearchType : byte
    {
        [FriendlyName("Равен")]
        Equals = 0,

        [FriendlyName("Содержит")]
        Contains = 1
    }
}
