using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager
{
    public static class NumbersExtensions
    {
        //  TODO отрефакторить: избавится от копи0паста.
        public static byte? ToByteNullable(this string value)
        {
            return string.IsNullOrEmpty(value) ? null : value.ToByte();
        }
        public static byte ToByte(this string value)
        {
            return byte.TryParse(value, out var res) ? res : throw new InvalidCastException($"Invalid byte value '{value}'");
        }
        public static int ToInt(this string value)
        {
            return int.TryParse(value, out var res) ? res : throw new InvalidCastException($"Invalid Int value '{value}'");
        }
        public static int? ToIntNullable(this string value)
        {
            return string.IsNullOrEmpty(value) ? null : value.ToInt();
        }
    }
}
