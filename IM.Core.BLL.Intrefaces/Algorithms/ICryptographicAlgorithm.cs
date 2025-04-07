using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Algorithms
{
    /// <summary>
    /// Криптографический алгоритм
    /// </summary>
    public interface ICryptographicAlgorithm
    {
        /// <summary>
        /// Шифрование текста
        /// </summary>
        /// <param name="plainText">Входные данные</param>
        /// <param name="key">Ключ шифрования</param>
        /// <returns></returns>
        public string Encrypt(string plainText, string key);

        /// <summary>
        /// Расшифровка текста
        /// </summary>
        /// <param name="cipherText">Зашифрованные данные</param>
        /// <param name="key">Клюя шифрования</param>
        /// <returns></returns>
        public string Decrypt(string cipherText, string key);
    }
}
