using InfraManager.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.Core.Helpers
{
    public class TripleDesEncryption
    {
        private static readonly TripleDESCryptoServiceProvider __mDes = new TripleDESCryptoServiceProvider();

        private static readonly UTF8Encoding __mUtf8 = new UTF8Encoding();

        private static readonly byte[] __mKey = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        private static readonly byte[] __mIv = { 8, 7, 6, 5, 4, 3, 2, 1 };

        public static byte[] Encrypt(byte[] input)
        {
            return Transform(input, __mDes.CreateEncryptor(__mKey, __mIv));
        }

        public static byte[] Decrypt(byte[] input)
        {
            return Transform(input, __mDes.CreateDecryptor(__mKey, __mIv));
        }

        public static string Encrypt(string text)
        {
            if (text == null || text.Length < 1)
            {
                return string.Empty;
            }
            byte[] input = __mUtf8.GetBytes(text);
            byte[] output = Transform(input, __mDes.CreateEncryptor(__mKey, __mIv));
            return Convert.ToBase64String(output);
        }

        public static string Decrypt(string text)
        {
            try
            {
                if (text == null || text.Length < 1)
                {
                    return string.Empty;
                }
                byte[] input = Convert.FromBase64String(text);
                byte[] output = Transform(input, __mDes.CreateDecryptor(__mKey, __mIv));
                return __mUtf8.GetString(output);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return text;
            }

        }

        private static byte[] Transform(byte[] input, ICryptoTransform cryptoTransform)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                using (CryptoStream cryptStream = new CryptoStream(memStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptStream.Write(input, 0, input.Length);
                    cryptStream.FlushFinalBlock();
                    memStream.Position = 0;
                    byte[] result = memStream.ToArray();
                    memStream.Close();
                    cryptStream.Close();
                    return result;
                }
            }
        }
    }
}
