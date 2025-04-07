using System.Security.Cryptography;
using System.Text;

namespace InfraManager.BLL.Algorithms
{
    internal class HashAlgorithm_SHA512 : IHashAlgorithm
    {
        public byte[] ComputeHash(string inputData)
        {
            var inputByties = Encoding.UTF8.GetBytes(inputData);

            using (var sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(inputByties);
            }
        }
    }
}
