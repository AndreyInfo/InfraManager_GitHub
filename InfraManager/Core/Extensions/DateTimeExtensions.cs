using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime? ToLocalTime(this DateTime? value)
        {
            return value == null ? null : value.Value.ToLocalTime();
        }
        public static DateTime? ToUniversalTime(this DateTime? value)
        {
            return value == null ? null : value.Value.ToUniversalTime();
        }
    }
}
