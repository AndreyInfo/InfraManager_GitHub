using System.Security.Cryptography;
using System.Text;

namespace InfraManager.BLL.Algorithms
{
    internal class HashAlgorithm_SHA1 : IHashAlgorithm
    {
        public byte[] ComputeHash(string inputData)
        {
            var inputByties = Encoding.UTF8.GetBytes(inputData);

            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(inputByties);
            }
        }
    }
}
