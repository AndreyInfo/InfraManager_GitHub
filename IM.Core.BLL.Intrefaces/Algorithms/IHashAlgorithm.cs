using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Algorithms
{
    /// <summary>
    /// Алгоритм хэширования
    /// </summary>
    public interface IHashAlgorithm
    {
        /// <summary>
        /// Вычисление хэша
        /// </summary>
        /// <param name="inputData">Входные данные</param>
        /// <returns></returns>
        public byte[] ComputeHash(string inputData);
    }
}
