using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Accounts
{
    public enum UserAccountTypes
    {
        /// <summary>
        /// Общего назначения
        /// </summary>
        Common = 1,

        /// <summary>
        /// Приложение
        /// </summary>
        Application = 2,

        /// <summary>
        /// Windows
        /// </summary>
        Windows = 3,

        /// <summary>
        /// VMware
        /// </summary>
        VMware = 4,

        /// <summary>
        /// SSH 
        /// </summary>
        SSH = 5,

        /// <summary>
        /// CIM
        /// </summary>
        CIM = 6,

        /// <summary>
        /// SNMP v2
        /// </summary>
        SNMPv2 = 7,

        /// <summary>
        /// SNMP v3
        /// </summary>
        SNMPv3 = 8
    }
}
