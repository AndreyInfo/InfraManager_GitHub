using InfraManager.BLL.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    public enum PolicyType : byte
    {
        [FriendlyName("Разрешить")]
        Allow,

        [FriendlyName("Запретить")]
        Deny,
    }
}
