using System.Security.Cryptography;
using System.Text;

namespace InfraManager.BLL.Algorithms
{
    internal class HashAlgorithm_SHA384 : IHashAlgorithm
    {
        public byte[] ComputeHash(string inputData)
        {
            var inputByties = Encoding.UTF8.GetBytes(inputData);

            using (var sha384 = SHA384.Create())
            {
                return sha384.ComputeHash(inputByties);
            }
        }
    }
}
