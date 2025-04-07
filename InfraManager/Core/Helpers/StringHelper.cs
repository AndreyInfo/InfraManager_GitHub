using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace InfraManager.Core.Helpers
{
    public static class StringHelper
    {

        #region static method Encrypt
        public static string Encrypt(string clearText)
        {
            string encryptionKey = "EBWKHzUb67W47mcgPVsTv4MWSYOEDebHV5IGBSfHDDzLgfkFh5exhCLluNYV";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        #endregion

        #region static method Decrypt
        public static string Decrypt(string cipherText)
        {
            string encryptionKey = "EBWKHzUb67W47mcgPVsTv4MWSYOEDebHV5IGBSfHDDzLgfkFh5exhCLluNYV";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion


        #region static EncodeBase64
        public static string EncodeBase64(Encoding encoding, string text)
        {
            if (text == null)
                return null;
            //
            byte[] textAsBytes = encoding.GetBytes(text);
            return Convert.ToBase64String(textAsBytes);
        }
        #endregion

        #region static DecodeBase64
        public static string DecodeBase64(Encoding encoding, string encodedText)
        {
            if (encodedText == null)
                return null;
            //
            byte[] textAsBytes = Convert.FromBase64String(encodedText);
            return encoding.GetString(textAsBytes);
        }
        #endregion
    }
}
