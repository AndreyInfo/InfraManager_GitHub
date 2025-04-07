using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.Core.Extensions
{
    public static class DecimalExtenshions
    {
        #region To2digits
        public static decimal To2digits(this decimal d)
        {
            return Decimal.Truncate(d * 100) / 100;
        }

        public static decimal? To2digits(this decimal? d)
        {
            if (d.HasValue)
                return Decimal.Truncate(d.Value * 100) / 100;
            else return null;
        }
        #endregion
    }
}
