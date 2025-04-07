using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    public interface IFindNameByGlobalID
    {
        /// <summary>
        /// Ищет имя сущности по значению глобального идентификатора
        /// </summary>
        /// <param name="keys">Глобальный идентификатор</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Значение имени сущности, пустую строку если имя равно NULL и null если сущность не найдена</returns>
        Task<string> FindAsync(Guid id, CancellationToken token = default);

        /// <summary>
        /// Ищет имя сущности по значению глобального идентификатора
        /// </summary>
        /// <param name="keys">Глобальный идентификатор</param>
        /// <returns>Значение имени сущности, пустую строку если имя равно NULL и null если сущность не найдена</returns>
        string Find(Guid id);
    }
}
