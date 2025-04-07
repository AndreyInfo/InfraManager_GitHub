using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    public interface IFindName
    {
        /// <summary>
        /// Ищем имя сущности по значению ключа
        /// </summary>
        /// <param name="keys">Ключ</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Значение имени сущности, пустую строку если имя равно NULL и null если сущность не найдена</returns>
        Task<string> FindAsync(object[] keys, CancellationToken token = default);

        /// <summary>
        /// Ищем имя сущности по значению ключа
        /// </summary>
        /// <param name="keys">Ключ</param>
        /// <returns>Значение имени сущности, пустую строку если имя равно NULL и null если сущность не найдена</returns>
        string Find(params object[] keys);
    }
}
