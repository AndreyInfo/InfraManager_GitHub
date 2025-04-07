using System;

namespace InfraManager.DAL.PGSQL
{
    public  static class RowVersionConverter
    {
        public static byte[] ConvertFromPostgreXid(this ulong value)
        {
            return new byte[]
            {
                (byte)(value >> 56),
                (byte)(value >> 48),
                (byte)(value >> 40),
                (byte)(value >> 32),
                (byte)(value >> 24),
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)value
            };

        }

        public static ulong ConvertToPostgreXid(this byte[] value)
        {
            if (value != null && value.Length == 8)
            {
                return ((ulong)value[0] << 56) |
                       ((ulong)value[1] << 48) |
                       ((ulong)value[2] << 40) |
                       ((ulong)value[3] << 32) |
                       ((ulong)value[4] << 24) |
                       ((ulong)value[5] << 16) |
                       ((ulong)value[6] << 8) |
                       ((ulong)value[7]);
            }
            else throw new ArgumentException(nameof(value));

        }
    }
}
