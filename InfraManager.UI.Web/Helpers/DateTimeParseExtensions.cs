using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Helpers
{
    public static class DateTimeParseExtensions
    {
        public static DateTime? ParseAsDate(this string value)
        {
            if (DateTime.TryParse(value, out var _date))
            {
                return _date;
            }
            if (long.TryParse(value, out var _long))
            {
                return (new DateTime(1970, 1, 1)).AddMilliseconds((double)_long);
            }
            return null;
        }
    }
}
