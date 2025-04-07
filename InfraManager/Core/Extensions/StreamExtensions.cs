using System;
using System.IO;

namespace InfraManager.Core.StreamExtensions
{
    public static class StreamExtensions
    {
        public static byte[] ToArray(this Stream stream)
        { 
            if (stream == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
