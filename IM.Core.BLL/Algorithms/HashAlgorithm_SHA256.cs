using System.Security.Cryptography;
using System.Text;

namespace InfraManager.BLL.Algorithms
{
    internal class HashAlgorithm_SHA256 : IHashAlgorithm
    {
        public byte[] ComputeHash(string inputData)
        {
            var inputByties = Encoding.UTF8.GetBytes(inputData);

            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(inputByties);
            }
        }
    }
}
