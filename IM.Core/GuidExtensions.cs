using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager
{
    public static class GuidExtensions
    {
        public static Guid? ToGuidNullable(this string value)
        {
            return string.IsNullOrEmpty(value) ? null : value.ToGuid();
        }
        public static Guid ToGuid(this string value)
        {
            return Guid.TryParse(value, out var res) ? res : throw new InvalidCastException($"can not cast to GUID value '{value}'");
        }
    }
}
