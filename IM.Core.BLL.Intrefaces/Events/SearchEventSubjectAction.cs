using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Events
{
    public enum SearchEventSubjectAction
    {
        /// <summary>
        /// Равно
        /// </summary>
        Equals = 0,
        /// <summary>
        /// Не равно
        /// </summary>
        NotEquals = 1,
        /// <summary>
        /// Содержит
        /// </summary>
        Contains= 2,
        /// <summary>
        /// Не содержит
        /// </summary>
        NotContains = 3
    }
}
