using System.Security.Cryptography;
using System.Text;

namespace InfraManager.BLL.Algorithms
{
    /// <summary>
    /// The hash size for the MD5 algorithm is 128 bits
    /// </summary>
    internal class HashAlgorithm_MD5 : IHashAlgorithm
    {
        public byte[] ComputeHash(string inputData)
        {
            var inputByties = Encoding.UTF8.GetBytes(inputData);

            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(inputByties);
            }
        }
    }
}
